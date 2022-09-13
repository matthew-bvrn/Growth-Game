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
	internal ESelectableState State { get; set; }

	protected bool m_canPlace = false;

	protected abstract void UpdateObject(RaycastHit[] hit);

	void Update()
	{
		if (State == ESelectableState.Moving)
		{
			Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
			RaycastHit[] hits = Physics.RaycastAll(ray, 100, ~LayerMask.GetMask("Object", "Selectable"));

			if (hits.Length != 0)
			{
				UpdateObject(hits);

				if (InputManager.Get.IsJustPressed(EActions.PlaceObject) && m_canPlace)
				{
					State = ESelectableState.Default;
					StateManager.Get.TrySetState(EGameState.Viewing);
				}
			}
		}
	}
}

