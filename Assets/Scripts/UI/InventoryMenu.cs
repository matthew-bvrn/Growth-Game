using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : ItemMenu
{
	[SerializeField] UiButton m_placeButton;


	protected override void AdditionalOnActivated()
	{
		m_placeButton.gameObject.SetActive(false);
	}

	public override void OnElementClicked()
	{
		m_placeButton.gameObject.SetActive(true);
		base.OnElementClicked();
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
