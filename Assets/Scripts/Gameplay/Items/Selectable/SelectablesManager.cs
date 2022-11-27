using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablesManager : MonoBehaviour
{
	public static SelectablesManager Get;


	public SelectableBase Selected { get; private set; }

	// Start is called before the first frame update
	void Start()
	{
		if (Get == null)
		{
			Get = this;
			HighlightablesManager.Get.SelectedEvent += OnSelected;
		}
		else
		{
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (StateManager.Get.State == EGameState.Viewing || StateManager.Get.State == EGameState.ObjectSelected)
		{
			if (InputManager.Get.IsJustPressed(EActions.CameraMoving))
			{
				StateManager.Get.TrySetState(EGameState.CameraMoving);
			}
		}
	}

	public void SetObjectMovingState(SelectableBase selectable = null)
	{
		if (selectable != null)
		{
			Selected = selectable;
		}

		Selected.State = ESelectableState.Moving;
		StateManager.Get.TrySetState(EGameState.ObjectMoving);
	}

	void OnSelected(GameObject selected)
	{
		if (StateManager.Get.State == EGameState.Viewing || StateManager.Get.State == EGameState.ObjectSelected)
		{
			if (selected != null)
			{
				if (selected.GetComponent<SelectableBase>() != null)
				{
					StateManager.Get.TrySetState(EGameState.ObjectSelected);
					Selected = selected.GetComponent<SelectableBase>();
				}
				else
				{
					Debug.LogError("Tried to select the parent of " + selected.name + ", but the parent is not a selectable object.");
				}
			}
			else
			{
				Selected = null;
				StateManager.Get.TrySetState(EGameState.Viewing);
			}
		}
	}
}
