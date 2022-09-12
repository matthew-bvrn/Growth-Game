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

enum EDirection
{
	None,
	EBidirectional,
	EPositive,
	ENegative
}

internal abstract class InputAxis : InputBase
{
	public abstract float GetAxis();
	public abstract float GetAxisPositive();
	public abstract float GetAxisNegative();

	public EDirection m_direction;
}

public class InputManager
{
	Dictionary<EActions, InputBase> m_inputs;
	InputImpl m_inputImplementation;

	public static InputManager Get { get => StateManager.Get.m_inputManager; }

	internal void Initialise(InputImpl impl)
	{
		m_inputImplementation = impl;
		m_inputImplementation.Initialise(ref m_inputs);
	}

	public Vector2 GetSelectionPosition()
	{
		return m_inputImplementation.GetSelectionPosition();
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

	public float GetAxis(EActions action)
	{
		InputBase input = GetInput(action);

		if (!CheckType(input, typeof(InputPress)))
			return 0;

		switch(((InputAxis)input).m_direction)
		{
			case EDirection.EBidirectional:
				return ((InputAxis)input).GetAxis();
			case EDirection.EPositive:
				return ((InputAxis)input).GetAxisPositive();
			case EDirection.ENegative:
				return ((InputAxis)input).GetAxisNegative();
			default:
				Debug.LogError("Axis doesn't have a direction defined");
				return ((InputAxis)input).GetAxis();
		}
	}
	
	bool CheckType(InputBase input, Type type)
	{
		if (input.GetType().IsSubclassOf(type))
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
