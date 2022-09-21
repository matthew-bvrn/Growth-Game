using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;
	[SerializeField] ScrollRect m_scrollView;
	[SerializeField] RectTransform m_elementTemplate;
	[SerializeField] Button m_placeButton;

	List<GameObject> m_elements = new List<GameObject>();

	string m_selectedGuid;
	GameObject m_sampleObject;

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	void OnStateChanged(EGameState state)
	{
		if(state == EGameState.OpenInventoryPressed)
		{
			StateManager.Get.TrySetState(EGameState.InventoryOpen);
		}
		else if (state == EGameState.InventoryOpen)
		{
			m_menu.SetActive(true);
			m_placeButton.gameObject.SetActive(false);
			CreateMenuElements();
		}
		else
		{
			RemoveSampleObject();
			m_menu.SetActive(false);
		}
	}

	void CreateMenuElements()
	{
		foreach (GameObject element in m_elements)
		{
			Destroy(element);
		}

		m_elements.Clear();

		foreach(ItemData item in InventoryManager.Get.Items)
		{
			GameObject element = Instantiate(m_elementTemplate.gameObject, m_scrollView.content);
			element.SetActive(true);
			element.GetComponentInChildren<Text>().text = item.Name;
			element.GetComponent<ItemUiElement>().Guid = item.Guid;
			m_elements.Add(element);
		}
	}

	public void OnElementClicked()
	{
		m_placeButton.gameObject.SetActive(true);

		m_selectedGuid = EventSystem.current.currentSelectedGameObject.GetComponent<ItemUiElement>().Guid;
		RemoveSampleObject();
		m_sampleObject = Instantiate(ItemLookupManager.Get.LookupItem(m_selectedGuid));

		foreach (MeshRenderer renderer in m_sampleObject.GetComponentsInChildren<MeshRenderer>())
		{
			if(renderer.gameObject.layer!= 9)
				renderer.gameObject.layer = 11;
		}

		m_sampleObject.transform.position += m_sampleObject.GetComponent<ItemComponent>().Offset;
		m_sampleObject.transform.rotation = Quaternion.Euler(m_sampleObject.GetComponent<ItemComponent>().Rotation);
	}

	public void OnPlaceButton()
	{
		InventoryManager.Get.PlaceItem(m_selectedGuid);
	}

	void RemoveSampleObject()
	{
		if(m_sampleObject!=null)
		{
			Destroy(m_sampleObject);
			m_sampleObject = null;
		}
	}

	public void Update()
	{
		if (StateManager.Get.State != EGameState.InventoryOpen)
			return;

		if (InputManager.Get.IsJustPressed(EActions.Select) && !HighlightSystem.Get.ElementHighlighted)
		{
			StateManager.Get.TrySetState(EGameState.Viewing);
		}

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving) && !HighlightSystem.Get.ElementHighlighted)
		{
			StateManager.Get.TrySetState(EGameState.CameraMoving);
		}
	}
}
