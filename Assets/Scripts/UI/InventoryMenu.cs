using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;
	[SerializeField] RectTransform m_elementTemplate;

	List<GameObject> m_elements = new List<GameObject>();

	GameObject m_sampleObject;

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	void OnStateChanged(EGameState state)
	{
		if (state == EGameState.InventoryOpen)
		{
			m_menu.SetActive(true);
			CreateMenuElements();
		}
		else
			m_menu.SetActive(false);
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
			GameObject element = Instantiate(m_elementTemplate.gameObject, m_menu.GetComponent<ScrollRect>().content);
			element.SetActive(true);
			element.GetComponentInChildren<Text>().text = item.Name;
			element.GetComponent<ItemUiElement>().Guid = item.Guid;
			m_elements.Add(element);
		}
	}

	public void OnElementClicked()
	{
		RemoveSampleObject();
		GameObject gameObject = ItemLookupManager.Get.LookupItem(EventSystem.current.currentSelectedGameObject.GetComponent<ItemUiElement>().Guid);
		m_sampleObject = Instantiate(gameObject);

		foreach (MeshRenderer renderer in m_sampleObject.GetComponentsInChildren<MeshRenderer>())
		{
			if(renderer.gameObject.layer!= 9)
				renderer.gameObject.layer = 11;
		}

		m_sampleObject.transform.position += m_sampleObject.GetComponent<ItemComponent>().Offset;
		m_sampleObject.transform.rotation = Quaternion.Euler(m_sampleObject.GetComponent<ItemComponent>().Rotation);
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
			RemoveSampleObject();
			StateManager.Get.TrySetState(EGameState.Viewing);
		}

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving) && !HighlightSystem.Get.ElementHighlighted)
		{
			RemoveSampleObject();
			StateManager.Get.TrySetState(EGameState.CameraMoving);
		}
	}
}
