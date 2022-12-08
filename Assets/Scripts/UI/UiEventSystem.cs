using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiEventSystem : MonoBehaviour
{
	public static UiEventSystem Get;

	internal UiButton Selected;
	internal UiButton SelectedTab;
	public bool ElementHighlighted { get => Highlighted != null || HighlightedTab != null; }
	internal UiButton Highlighted = null;
	internal UiButton HighlightedTab = null;

	internal Vector3 SelectedMousePos { get; private set; }

	private void Start()
	{
		if (Get == null)
			Get = this;
		else
			Destroy(this);
	}

	internal void Highlight(UiButton button)
	{
		if (button.Tag == ButtonTag.Button)
			Highlighted = button;
		else if (button.Tag == ButtonTag.Tab)
			HighlightedTab = button;

		Debug.Log("Highlighted: " + button);

		HighlightablesManager.Get.CanSelect = false;
	}

	internal void Unhighlight(UiButton button)
	{
		Debug.Log("Highlighted: none");

		if (button == Highlighted)
		{
			if (Highlighted != Selected)
				Highlighted.SetState(ButtonState.Normal);

			Highlighted = null;
			HighlightablesManager.Get.CanSelect = true;
		}
		else if (button == HighlightedTab)
		{
			if (HighlightedTab != SelectedTab)
				HighlightedTab.SetState(ButtonState.Normal);

			HighlightedTab = null;
			HighlightablesManager.Get.CanSelect = true;
		}
	}

	internal void Unselect(ButtonTag tag)
	{
		if (tag == ButtonTag.Button)
			Unselect(Selected);
		else
			Unselect(SelectedTab);
	}

	internal void Unselect(UiButton button)
	{
		SelectedMousePos = Input.mousePosition;

		if (button == null)
			return;

		if (button.Tag == ButtonTag.Button)
			Selected = null;
		else
			SelectedTab = null;

		button.SetState(ButtonState.Normal);
	}

	internal void Select(UiButton button)
	{
		SelectedMousePos = Input.mousePosition;

		Unselect(button.Tag);

		if (button.Tag == ButtonTag.Button)
			Selected = button;
		else
			SelectedTab = button;

		Unhighlight(button);
		button.m_onSelected.Invoke();
	}
}
