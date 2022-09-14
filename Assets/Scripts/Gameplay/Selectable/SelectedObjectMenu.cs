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

	public void OnMove()
	{
		SelectablesManager.Get.Selected.State = ESelectableState.Moving;
		StateManager.Get.TrySetState(EGameState.ObjectMoving);
	}

	void OnStateChange(EGameState state)
	{
		if(state == EGameState.ObjectSelected)
		{
			m_menu.SetActive(true);
			m_menu.GetComponent<RectTransform>().position = InputManager.Get.GetSelectionPosition(); 

		}
		else
		{
			m_menu.SetActive(false);
		}
	}
}