using System.Collections;
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

internal class MouseAxis : InputAxis
{
	public MouseAxis(string axisName, EDirection direction) { m_axisName = axisName; m_direction = direction; }

	public override float GetAxis()
	{
		return Input.GetAxis(m_axisName);
	}

	public override float GetAxisPositive()
	{
		return Mathf.Max(0,Input.GetAxis(m_axisName));
	}

	public override float GetAxisNegative()
	{
		return -Mathf.Min(0,Input.GetAxis(m_axisName));
	}

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

		inputs.Add(EActions.LookActive, new MouseButtonPress(1));
		inputs.Add(EActions.Rotate, new MouseAxis("Mouse X", EDirection.EBidirectional));
		inputs.Add(EActions.Height, new MouseAxis("Mouse Y", EDirection.EBidirectional));
	}
}
