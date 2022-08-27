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

internal class InputImplMouseKeyboard : InputImpl
{
  public void Initialise(ref Dictionary<EActions, InputBase> inputs)
	{
		//TODO move this to data
		inputs = new Dictionary<EActions, InputBase>();

		inputs.Add(EActions.ToggleConsole, new KeyboardButtonPress(KeyCode.Tab));
		inputs.Add(EActions.SubmitCommand, new KeyboardButtonPress(KeyCode.Return));
	}

	public void UpdateInputs(ref Dictionary<EActions, InputBase> inputs)
	{
	}
}
