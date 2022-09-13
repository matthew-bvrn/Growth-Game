using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESelectableState
{
	Default,
	Moving
}

public abstract class SelectableObject : MonoBehaviour
{
	internal ESelectableState State {get; set;}

	protected abstract bool CanPlace(RaycastHit hit);

	void Update()
	{
		if (State == ESelectableState.Moving)
		{
			RaycastHit hit;

			Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
			if (Physics.Raycast(ray, out hit, 100, ~LayerMask.GetMask("Object","Selectable")))
			{
				if (CanPlace(hit))
				{
					gameObject.transform.position = hit.point;

					if(InputManager.Get.IsJustPressed(EActions.PlaceObject))
					{
						State = ESelectableState.Default;
						StateManager.Get.TrySetState(EGameState.Viewing);
					}
				}

				float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

				if (rotate < 0)
					gameObject.transform.Rotate(new Vector3(0, 15, 0));
				if(rotate > 0)
					gameObject.transform.Rotate(new Vector3(0, -15, 0));
			}
		}
	}
}

