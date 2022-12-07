using UnityEngine;

public class InventoryMenu : UiItemScrollView
{
	[SerializeField] UiButton m_placeButton;


	protected override void AdditionalOnActivated()
	{
		m_placeButton.gameObject.SetActive(false);
	}

	protected override void AdditionalOnElementClicked()
	{
		m_placeButton.gameObject.SetActive(true);
	}

	protected override EGameState GetState()
	{
		return EGameState.InventoryOpen;
	}

	public void OnPlaceButton()
	{
		InventoryManager.Get.PlaceItem(m_itemGuid);
	}
}
