using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	void OnStateChanged(EGameState state)
	{
		if (state == EGameState.InventoryOpen)
			m_menu.SetActive(true);
		else
			m_menu.SetActive(false);
	}

	public void Update()
	{
		if (StateManager.Get.State != EGameState.InventoryOpen)
			return;

		if (InputManager.Get.IsJustPressed(EActions.Select) && !HighlightSystem.Get.ElementHighlighted)
			StateManager.Get.TrySetState(EGameState.Viewing);

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving) && !HighlightSystem.Get.ElementHighlighted)
			StateManager.Get.TrySetState(EGameState.CameraMoving);

	}
}
