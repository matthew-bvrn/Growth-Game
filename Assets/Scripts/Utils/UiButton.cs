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

internal enum ButtonAppearanceChange
{
	Colour,
	Sprites
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

	[SerializeField] internal ButtonAppearanceChange m_buttonAppearanceChange;

	public Color m_normalColor = new Color(1, 1, 1);
	public Color m_highlightedColor = new Color(1, 0.6f, 0.6f);
	public Color m_selectedColor = new Color(0.6f, 0.6f, 1);
	public Color m_pressedColor = new Color(1, 0, 0);

	public Texture m_normalTexture;
	public Texture m_highlightedTexture;
	public Texture m_pressedTexture;

	public float m_fadeDuration = 0.1f;

	float m_animateTime = 0f;
	public float m_animateDelay = 0f;
	const float m_animateEndTime = 0.25f;

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
				UiEventSystem.Get.OnHighlighted(this);
			}
		}
		if (!m_mouseInRegion && (State == ButtonState.Highlighted || State == ButtonState.Pressed))
		{
			if (ToggleState == true)
				SetState(ButtonState.Selected);
			else
				SetState(ButtonState.Normal);
			UiEventSystem.Get.OnUnhighlighted(this);
		}

		if (m_timer < m_fadeDuration && m_buttonAppearanceChange == ButtonAppearanceChange.Colour)
		{
			GetComponent<Image>().color = (m_timer / m_fadeDuration) * m_newColor + (1 - m_timer / m_fadeDuration) * m_previousColor;
			m_timer += Time.deltaTime;
		}

		if(m_animateTime < m_animateEndTime + m_animateDelay)
		{
			m_animateTime += Time.deltaTime;

			if (m_animateTime > m_animateDelay)
			{
				float progress = (m_animateTime-m_animateDelay) / m_animateEndTime;

				float size = 1 + 1.9f * Mathf.Pow(progress - 1, 3.0f) + 0.9f * Mathf.Pow(progress - 1, 2.0f);

				GetComponent<RectTransform>().localScale = new Vector3(size, size, size);
			}
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
					if (m_buttonAppearanceChange == ButtonAppearanceChange.Colour)
						m_newColor = m_normalColor;
					else
						GetComponent<RawImage>().texture = m_normalTexture;
					break;
				case ButtonState.Highlighted:
					if (m_buttonAppearanceChange == ButtonAppearanceChange.Colour)
						m_newColor = m_highlightedColor;
					else
						GetComponent<RawImage>().texture = m_highlightedTexture;
					break;
				case ButtonState.Pressed:
					if (m_buttonAppearanceChange == ButtonAppearanceChange.Colour)
						m_newColor = m_pressedColor;
					else
						GetComponent<RawImage>().texture = m_pressedTexture;
					break;
				case ButtonState.Selected:
					m_newColor = m_selectedColor;
					break;
			}

			m_timer = 0;
			State = state;
		}
	}

	public void RestartAnimation()
	{
		m_animateTime = 0;
		GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
	}

	void OnEnable()
	{
		if (m_buttonAppearanceChange == ButtonAppearanceChange.Colour)
		{
			if (UiEventSystem.Get.Highlighted == this)
				GetComponent<Image>().color = m_highlightedColor;
			else if (UiEventSystem.Get.Selected == this)
				GetComponent<Image>().color = m_selectedColor;
			else
				GetComponent<Image>().color = m_normalColor;
		}

		RestartAnimation();
	}

	void OnDisable()
	{
		UiEventSystem.Get.OnUnhighlighted(this);
	}
}
