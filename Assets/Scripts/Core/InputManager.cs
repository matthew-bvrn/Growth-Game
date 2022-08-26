using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface InputBase
{
}

internal interface InputPress : InputBase
{
	public bool IsPressed();
	public bool IsJustPressed();
	public bool IsJustReleased();
}

internal interface InputAxis : InputBase
{
	public float GetX();
	public float GetY();
	public bool IsActivated();
}

public class InputManager : MonoBehaviour
{
	Dictionary<EActions, InputBase> m_inputs;
	InputImpl m_inputImplementation;

	internal void Initialise(InputImpl impl)
	{
		m_inputImplementation = impl;
		m_inputImplementation.Initialise(ref m_inputs);
	}

	public void UpdateInputs()
	{
		m_inputImplementation.UpdateInputs(ref m_inputs);
	}

	public bool IsPressed(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return false;

		return ((InputPress)input).IsPressed();
	}

	public bool IsJustPressed(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return false;

		return ((InputPress)input).IsJustPressed();
	}

	public bool IsJustReleased(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return false;

		return ((InputPress)input).IsJustReleased();
	}

	public float GetX(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return 0;

		return ((InputAxis)input).GetX();
	}

	public float GetY(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return 0;

		return ((InputAxis)input).GetY();
	}

	public bool IsActivated(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return false;

		return ((InputAxis)input).IsActivated();
	}
	
	bool CheckType(InputBase input, Type type)
	{
		if (input.GetType() != type)
		{
			Debug.LogError("Input is being used like a " + type.ToString() + " but it is actually a " + input.GetType().ToString());
			return false;
		}

		return true;
	}

	InputBase GetInput(EActions action)
	{
		InputBase input;
		m_inputs.TryGetValue(action, out input);
		if (input == null)
			Debug.LogError("EAction " + action + " does not have a mapping");
		return input;
	}

}
