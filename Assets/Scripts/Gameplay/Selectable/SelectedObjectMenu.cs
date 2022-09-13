using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;

	// Start is called before the first frame update
	void Start()
	{
		StateManager.Get.OnStateChange += OnStateChange;
	}

	void OnStateChange(EGameState state)
	{
		if(state == EGameState.ObjectSelected)
		{
			SelectableObject selected = SelectablesManager.Get.Selected;
			m_menu.SetActive(true);
			m_menu.GetComponent<RectTransform>().position = InputManager.Get.GetSelectionPosition(); 

		}
		else
		{
			m_menu.SetActive(false);
		}
	}
}
