using System;
using System.Collections.Generic;
using UnityEngine;

internal class KeyboardButtonPress : InputPress
{
	public KeyboardButtonPress(KeyCode key) => m_keyCode = key;

	public bool IsPressed()
	{
		return Input.GetKey(m_keyCode);
	}

	public bool IsJustPressed()
	{
		return Input.GetKeyDown(m_keyCode);
	}

	public bool IsJustReleased()
	{
		return Input.GetKeyUp(m_keyCode);
	}

	KeyCode m_keyCode;
}

internal class MouseButtonPress : InputPress
{
	public MouseButtonPress(int button) => m_button = button;

	public bool IsPressed()
	{
		return Input.GetMouseButton(m_button);
	}

	public bool IsJustPressed()
	{
		return Input.GetMouseButtonDown(m_button);
	}

	public bool IsJustReleased()
	{
		return Input.GetMouseButtonUp(m_button);
	}

	int m_button;
}

enum EOperator
{
	And, Or
}

internal class InputPressCollection : InputPress
{
	public InputPressCollection(List<InputPress> inputs, EOperator _operator) { m_inputs = inputs; m_operator = _operator; }

	public bool IsPressed()
	{
		switch (m_operator)
		{
			case EOperator.And:
				foreach (InputPress press in m_inputs)
					if (!press.IsPressed())
						return false;
				return true;
			case EOperator.Or:
				foreach (InputPress press in m_inputs)
					if (press.IsPressed())
						return true;
				return false;
			default:
				return false;
		}
	}

	public bool IsJustPressed()
	{
		switch (m_operator)
		{
			case EOperator.And:
				foreach (InputPress press in m_inputs)
					if (!press.IsJustPressed())
						return false;
				return true;
			case EOperator.Or:
				foreach (InputPress press in m_inputs)
					if (press.IsJustPressed())
						return true;
				return false;
			default:
				return false;
		}
	}

	public bool IsJustReleased()
	{
		switch (m_operator)
		{
			case EOperator.And:
				foreach (InputPress press in m_inputs)
					if (!press.IsJustReleased())
						return false;
				return true;
			case EOperator.Or:
				foreach (InputPress press in m_inputs)
					if (press.IsJustReleased())
						return true;
				return false;
			default:
				return false;
		}
	}

	List<InputPress> m_inputs;
	EOperator m_operator;
}

internal class MouseAxis : InputAxis
{
	public MouseAxis(string axisName, EDirection direction) { m_axisName = axisName; m_direction = direction; }
	public MouseAxis(string axisName, EDirection direction, InputPress press) { m_axisName = axisName; m_direction = direction; m_pressPrerequisite = press; }


	public override float GetAxis()
	{
		if(m_pressPrerequisite != null && !m_pressPrerequisite.IsPressed())
			return 0;

		return Input.GetAxis(m_axisName);
	}

	public override float GetAxisPositive()
	{
		if (m_pressPrerequisite != null && !m_pressPrerequisite.IsPressed())
			return 0;

		return Mathf.Max(0,Input.GetAxis(m_axisName));
	}

	public override float GetAxisNegative()
	{
		if (m_pressPrerequisite != null && !m_pressPrerequisite.IsPressed())
			return 0;

		return -Mathf.Min(0,Input.GetAxis(m_axisName));
	}

	InputPress m_pressPrerequisite = null;
	string m_axisName;
}

internal class InputImplMouseKeyboard : InputImpl
{
  public void Initialise(ref Dictionary<EActions, InputBase> inputs)
	{
		inputs = new Dictionary<EActions, InputBase>();

		inputs.Add(EActions.ToggleConsole, new KeyboardButtonPress(KeyCode.Tab));
		inputs.Add(EActions.SubmitCommand, new KeyboardButtonPress(KeyCode.Return));

		inputs.Add(EActions.ScrollUp, new KeyboardButtonPress(KeyCode.UpArrow));
		inputs.Add(EActions.ScrollDown, new KeyboardButtonPress(KeyCode.DownArrow));

		inputs.Add(EActions.ZoomIn, new MouseAxis("Mouse ScrollWheel", EDirection.EPositive));
		inputs.Add(EActions.ZoomOut, new MouseAxis("Mouse ScrollWheel", EDirection.ENegative));

		inputs.Add(EActions.CameraMoving, new InputPressCollection(new List<InputPress> { new MouseButtonPress(1), new MouseButtonPress(2) }, EOperator.Or));
		inputs.Add(EActions.RotateX, new MouseAxis("Mouse X", EDirection.EBidirectional, new MouseButtonPress(2)));
		inputs.Add(EActions.ChangeHeight, new MouseAxis("Mouse Y", EDirection.EBidirectional, new MouseButtonPress(1)));
		inputs.Add(EActions.RotateY, new MouseAxis("Mouse Y", EDirection.EBidirectional, new MouseButtonPress(2)));

		inputs.Add(EActions.SelectObject, new MouseButtonPress(0));
	}

	public Vector2 GetSelectionPosition()
	{
		return Input.mousePosition;
	}
}
