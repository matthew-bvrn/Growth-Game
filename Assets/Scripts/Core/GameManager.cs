using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	internal InputManager m_inputManager;

	GameManager()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Debug.LogError("GameManager can only have one instance!");
			Object.Destroy(this);
		}
	}

	void Start()
	{
		m_inputManager = new InputManager();
		m_inputManager.Initialise(new InputImplMouseKeyboard());
	}

	// Update is called once per frame
	void Update()
	{
		m_inputManager.UpdateInputs();
	}
}
