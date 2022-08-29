using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ParamsAction(int count, params string[] arguments);

public class GameConsole : MonoBehaviour
{
	public static GameConsole Instance { get; private set; }

	GameConsole()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Debug.LogError("GameConsole can only have one instance!");
			Object.Destroy(this);
		}
	}

	[SerializeField] InputField m_console;

	Dictionary<string, ParamsAction> m_commands = new Dictionary<string, ParamsAction>();

	public void AddCommand(string name, ParamsAction action)
	{
		if(m_commands.ContainsKey(name))
		{
			Debug.LogError("There is already a command named " + name);
			return;
		}

		m_commands.Add(name, action);
	}

	void Update()
	{
		if (InputManager.Get.IsJustPressed(EActions.ToggleConsole))
		{
			if (!m_console.gameObject.activeInHierarchy && GameManager.Get.TrySetState(GameState.Console))
			{
				m_console.gameObject.SetActive(true);
				m_console.ActivateInputField();
			}

			else if (m_console.gameObject.activeInHierarchy && GameManager.Get.TrySetState(GameState.Spectate))
				m_console.gameObject.SetActive(false);
		}

		if (m_console.gameObject.activeInHierarchy && InputManager.Get.IsJustPressed(EActions.SubmitCommand))
		{
			RunCommand(m_console.text);
			m_console.text = "";
			m_console.ActivateInputField();
		}
	}

	void RunCommand(string text)
	{
		if (text == "")
			return;

		List<string> words = text.Split().ToList<string>();
		string cmd = words[0];
		words.Remove(cmd);

		ParamsAction action;
		m_commands.TryGetValue(cmd, out action);
		if (action == null)
		{
			Debug.LogWarning("There is no command with the name " + cmd);
			return;
		}

		action(words.Count, words.ToArray());
	}
}
