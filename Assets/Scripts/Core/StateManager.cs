using System;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
	Viewing,
	CameraMoving,
	ObjectSelected,
	ObjectMoving,
	OpenInventoryPressed,
	InventoryOpen,
	Console,
	Inspecting
}

[Serializable]
public struct TransitionMap
{
	[SerializeField] public EGameState m_state;
	[SerializeField] public List<EGameState> m_transitions;
}

public delegate void StateChangeEvent(EGameState state);

public class StateManager : MonoBehaviour
{
	public static StateManager Get { get; private set; }

	internal InputManager m_inputManager;

	[SerializeField] EGameState m_state;
	[SerializeField] List<TransitionMap> m_transitions;

	EGameState m_prevState;

	public EGameState State { get => m_state; }
	public EGameState PreviousState { get => m_prevState; }
	public event StateChangeEvent OnStateChange;

	StateManager()
	{
		if (Get == null)
			Get = this;
		else
		{
			Debug.LogError("GameManager can only have one instance!");
			Destroy(this);
		}
	}

	void Start()
	{
		m_inputManager = new InputManager();
		m_inputManager.Initialise(new InputImplMouseKeyboard());
	}

	public bool TrySetState(EGameState state)
	{
		if (state == m_state)
		{
			return true;
		}

		TransitionMap map = m_transitions.Find((map) => map.m_state == m_state);

		if(map.m_transitions == null)
		{
			Debug.LogError("State " + m_state + " doesn't have any transitions defined.");
			return false;
		}

		if (!map.m_transitions.Contains(state))
		{
			Debug.LogWarning("Cannot transition from state " + m_state + " to " + state);
			return false;
		}

		m_prevState = m_state;
		m_state = state;
		OnStateChange.Invoke(state);
		return true;
	}
}
