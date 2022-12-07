using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class UiItemScrollView : MonoBehaviour
{
	[SerializeField] protected GameObject m_menu;
	[SerializeField] protected ScrollRect m_scrollView;
	[SerializeField] RectTransform m_elementTemplate;

	internal List<GameObject> m_elements = new List<GameObject>();
	GameObject m_sampleObject;
	protected string m_itemGuid;

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	virtual protected void AdditionalOnActivated() { }
	virtual protected void AdditionalOnElementClicked() { }
	abstract protected EGameState GetState();

	protected void OnStateChanged(EGameState state)
	{
		if (state == GetState())
		{
			m_menu.SetActive(true);
			AdditionalOnActivated();
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

	public void OnElementClicked()
	{
		AdditionalOnElementClicked();

		ItemUiElement selected = UiEventSystem.Get.Selected.GetComponent<ItemUiElement>();
		m_itemGuid = selected.ItemGuid;
		ItemData itemData = InventoryManager.Get.GetItem(m_itemGuid);

		RemoveSampleObject();
		m_sampleObject = Instantiate(ItemLookupManager.Get.LookupItem(selected.Guid));
		m_sampleObject.GetComponent<SelectableBase>().SetInventoryPreviewState();

		if (itemData.additionalData != null)
		{
			itemData.additionalData.LoadData(m_sampleObject);
		}

		foreach (MeshRenderer renderer in m_sampleObject.GetComponentsInChildren<MeshRenderer>())
		{
			if (renderer.gameObject.layer != 9)
				renderer.gameObject.layer = 11;
		}

		m_sampleObject.transform.position += m_sampleObject.GetComponent<ItemComponent>().Offset;
		m_sampleObject.transform.rotation = Quaternion.Euler(m_sampleObject.GetComponent<ItemComponent>().Rotation);
	}

	void RemoveSampleObject()
	{
		if (m_sampleObject != null)
		{
			Destroy(m_sampleObject);
			m_sampleObject = null;
		}
	}

	public void Update()
	{
		if (StateManager.Get.State != GetState())
			return;

		if (InputManager.Get.IsJustPressed(EActions.Select) && !UiEventSystem.Get.ElementHighlighted)
		{
			StateManager.Get.TrySetState(EGameState.Viewing);
		}

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving) && !UiEventSystem.Get.ElementHighlighted)
		{
			StateManager.Get.TrySetState(EGameState.CameraMoving);
		}
	}
}


