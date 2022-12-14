using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ParamsAction(int count, params string[] arguments);

public class GameConsole : MonoBehaviour
{
	public static GameConsole Instance { get; private set; }

	void Start()
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
	List<string> m_previousCommands = new List<string>();
	int m_previousCommandPos = -1;

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
#if DEBUG
		if (InputManager.Get.IsJustPressed(EActions.ToggleConsole))
		{
			if (!m_console.gameObject.activeInHierarchy && StateManager.Get.TrySetState(EGameState.Console))
			{
				m_console.gameObject.SetActive(true);
				m_console.ActivateInputField();
			}

			else if (m_console.gameObject.activeInHierarchy && StateManager.Get.TrySetState(EGameState.Viewing))
				m_console.gameObject.SetActive(false);
		}

		if (m_console.gameObject.activeInHierarchy && InputManager.Get.IsJustPressed(EActions.SubmitCommand))
		{
			m_previousCommands.Insert(0, m_console.text);
			if(m_previousCommands.Count == 51)
				m_previousCommands.RemoveAt(50);

			RunCommand(m_console.text);
			m_console.text = "";
			m_console.ActivateInputField();
			m_previousCommandPos = -1;
		}

		if(InputManager.Get.IsJustPressed(EActions.ScrollUp))
		{
			if (m_previousCommandPos == -1)
			{
				m_previousCommands.Insert(0, m_console.text);
				++m_previousCommandPos;
			}

			if (m_previousCommandPos < m_previousCommands.Count - 1)
				++m_previousCommandPos;
			
			m_console.text = m_previousCommands[m_previousCommandPos];
		}
		else if(InputManager.Get.IsJustPressed(EActions.ScrollDown))
		{
			if (m_previousCommandPos > 0)
				--m_previousCommandPos;

			m_console.text = m_previousCommands[m_previousCommandPos];
		}
#endif
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
