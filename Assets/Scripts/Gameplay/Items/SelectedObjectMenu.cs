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
		SelectablesManager.Get.OnSelected += OnSelected;
	}

	public void OnMove()
	{
		SelectablesManager.Get.SetObjectMovingState();
	}

	public void OnMoveToInventory()
	{
		InventoryManager.Get.MoveToInventory(SelectablesManager.Get.Selected.GetComponent<ItemComponent>());
		StateManager.Get.TrySetState(EGameState.Viewing);
	}

	void OnStateChange(EGameState state)
	{
		if (state != EGameState.ObjectSelected)
			m_menu.SetActive(false);
	}

	void OnSelected(SelectableBase selectable)
	{
		if(selectable)
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
