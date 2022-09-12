using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	Viewing,
	ObjectHighlighted,
	ObjectSelected,
	Console
}

[Serializable]
public struct TransitionMap
{
	[SerializeField] public GameState m_state;
	[SerializeField] public List<GameState> m_transitions;
}

public class StateManager : MonoBehaviour
{
	public static StateManager Get { get; private set; }

	internal InputManager m_inputManager;

	[SerializeField] GameState m_state;
	[SerializeField] List<TransitionMap> m_transitions;
	public GameState State { get => m_state; }

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

	public bool TrySetState(GameState state)
	{
		TransitionMap map = m_transitions.Find((map) => map.m_state == m_state);
		if (!map.m_transitions.Contains(state))
		{
			Debug.LogWarning("Cannot transition from state " + m_state + " to " + state);
			return false;
		}

		m_state = state;
		return true;
	}
}
