using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiEventSystem : MonoBehaviour
{
	public static UiEventSystem Get;

	public UiButton Selected { get; private set; }
	public bool ElementHighlighted { get => Highlighted != null; }
	public UiButton Highlighted { get; private set; } = null;

	internal Vector3 SelectedMousePos { get; private set; }

	private void Start()
	{
		if (Get == null)
			Get = this;
		else
			Destroy(this);
	}

	public void OnHighlighted(UiButton button)
	{
		Highlighted = button;
	}

	public void OnUnhighlighted(UiButton button)
	{
		if (button == Highlighted)
		{
			Highlighted.SetState(ButtonState.Normal);
			Highlighted = null;
		}
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0) && Highlighted != null && Highlighted.isActiveAndEnabled && Highlighted.State==ButtonState.Highlighted)
		{
			SelectedMousePos = Input.mousePosition;
			switch (Highlighted.Type)
			{
				case ButtonType.Selectable:
					if (Selected != null)
						Selected.SetState(ButtonState.Normal);
					Selected = Highlighted;
					Selected.SetState(ButtonState.Pressed);
					break;
				case ButtonType.Action:
					Highlighted.SetState(ButtonState.Pressed);
					break;
				case ButtonType.Toggle:
					Highlighted.SetState(ButtonState.Pressed);
					Highlighted.ToggleState = !Highlighted.ToggleState;
					if (Highlighted.ToggleState)
					{
						Selected = Highlighted;
					}
					else
					{
						Selected = null;
					}
					break;
			}
		}
		if (Input.GetMouseButtonUp(0) && Highlighted.State == ButtonState.Pressed)
		{
			if (Highlighted != null)
			{
				if (Highlighted.Type == ButtonType.Toggle)
				{
					if (Highlighted.ToggleState == true)
					{
						Highlighted.SetState(ButtonState.Selected);
						Highlighted.m_onSelected.Invoke();
						return;
					}
					else
					{
						Highlighted.SetState(ButtonState.Normal);
						Highlighted.m_onDeselected.Invoke();
						return;
					}
				}

				if (Highlighted.Type == ButtonType.Action)
				{
					Highlighted.SetState(ButtonState.Normal);
					Highlighted.m_onSelected.Invoke();
					return;
				}
			}
		}
	}
}
