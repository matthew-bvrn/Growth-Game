using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

internal enum ButtonType
{
	Inert,
	Selectable,
	Action,
	Toggle
}

internal enum ButtonState
{
	Normal,
	Highlighted,
	Selected,
	Pressed
}

[System.Serializable]
public class ButtonEvent : UnityEvent { }

public class UiButton : MonoBehaviour
{
	[SerializeField] internal ButtonType Type;
	internal ButtonState State;

	public Color m_normalColor = new Color(1, 1, 1);
	public Color m_highlightedColor = new Color(1, 0.6f, 0.6f);
	public Color m_selectedColor = new Color(0.6f, 0.6f, 1);
	public Color m_pressedColor = new Color(1, 0, 0);
	public float m_fadeDuration = 0.1f;

	public ButtonEvent m_onSelected;
	public ButtonEvent m_onDeselected;

	Color m_previousColor;
	Color m_newColor;
	float m_timer = 1;

	internal bool ToggleState = false;

	bool m_mouseInRegion;

	public void Update()
	{
		RectTransform rect = GetComponent<RectTransform>();

		//Debug.Log(rect.position.x+rect.rect.min.x);

		if (Input.mousePosition.x > rect.position.x + rect.rect.min.x && Input.mousePosition.x < rect.position.x + rect.rect.max.x
			&& Input.mousePosition.y > rect.position.y + rect.rect.min.y && Input.mousePosition.y < rect.position.y + rect.rect.max.y)
		{
			m_mouseInRegion = true;
		}
		else
		{
			m_mouseInRegion = false;
		}

		if (State != ButtonState.Pressed)
		{
			if (m_mouseInRegion && State != ButtonState.Highlighted && (UiEventSystem.Get.SelectedMousePos != Input.mousePosition))
			{
				SetState(ButtonState.Highlighted);
				Debug.Log("highlight");
				UiEventSystem.Get.OnHighlighted(this);
			}
			else if (!m_mouseInRegion && State == ButtonState.Highlighted)
			{
				if (ToggleState == true)
					SetState(ButtonState.Selected);
				else
					SetState(ButtonState.Normal);
				UiEventSystem.Get.OnUnhighlighted(this);
			}
		}

		if (m_timer < m_fadeDuration)
		{
			Debug.Log(GetComponent<Image>().color);
			GetComponent<Image>().color = (m_timer / m_fadeDuration) * m_newColor + (1 - m_timer / m_fadeDuration) * m_previousColor;
			m_timer += Time.deltaTime;
		}
	}

	internal void SetState(ButtonState state)
	{
		if (Type != ButtonType.Inert)
		{
			switch (State)
			{
				case ButtonState.Normal:
					m_previousColor = m_normalColor;
					break;
				case ButtonState.Highlighted:
					m_previousColor = m_highlightedColor;
					break;
				case ButtonState.Pressed:
					m_previousColor = m_pressedColor;
					break;
				case ButtonState.Selected:
					m_previousColor = m_selectedColor;
					break;
			}

			switch (state)
			{
				case ButtonState.Normal:
					m_newColor = m_normalColor;
					break;
				case ButtonState.Highlighted:
					m_newColor = m_highlightedColor;
					break;
				case ButtonState.Pressed:
					m_newColor = m_pressedColor;
					break;
				case ButtonState.Selected:
					m_newColor = m_selectedColor;
					break;
			}

			m_timer = 0;
			State = state;
		}
	}

	void OnEnable()
	{
		if (UiEventSystem.Get.Highlighted == this)
			GetComponent<Image>().color = m_highlightedColor;
		else if (UiEventSystem.Get.Selected == this)
			GetComponent<Image>().color = m_selectedColor;
		else
			GetComponent<Image>().color = m_normalColor;
	}

	void OnDisable()
	{
		UiEventSystem.Get.OnUnhighlighted(this);
	}
}
