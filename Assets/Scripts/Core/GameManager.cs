using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public InputManager InputManager { get; private set; }

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
		InputManager = new InputManager();
		InputManager.Initialise(new InputImplMouseKeyboard());
	}

	// Update is called once per frame
	void Update()
	{
		InputManager.UpdateInputs();
	}
}
