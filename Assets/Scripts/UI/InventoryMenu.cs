using UnityEngine;
using UnityEngine.UI;

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

	protected override void CreateMenuElements()
	{
		foreach (GameObject element in m_elements)
		{
			Destroy(element);
		}

		m_elements.Clear();

		foreach (ItemData item in InventoryManager.Get.Items)
		{
			ItemUiElement element = ((GameObject)Instantiate(m_elementTemplate.gameObject, m_scrollView.content)).GetComponent<ItemUiElement>();
			element.gameObject.SetActive(true);
			element.GetComponentInChildren<Text>().text = item.Name;
			element.Guid = item.Guid;
			element.ItemGuid = item.ItemGuid;
			element.Tags = item.Tags;
			m_elements.Add(element.gameObject);
		}
	}
}
