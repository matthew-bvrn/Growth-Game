using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
	void Update()
	{
		if (GameManager.Instance.InputManager.IsJustPressed(EActions.ToggleConsole))
			Debug.Log("Toggle console pressed");

	}
}
