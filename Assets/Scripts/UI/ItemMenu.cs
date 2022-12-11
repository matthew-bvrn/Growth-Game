using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class ItemMenu : MonoBehaviour
{
	[SerializeField] protected GameObject m_menu;
	[SerializeField] protected ScrollRect m_scrollView;
	[SerializeField] protected RectTransform m_elementTemplate;
	[SerializeField] protected ItemListManager m_itemListManager; 

	internal List<GameObject> m_elements = new List<GameObject>();
	GameObject m_sampleObject;
	protected string m_itemGuid;
	protected ItemUiElement m_selected;
	protected ItemData m_selectedData;

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	virtual protected void AdditionalOnActivated() { }
	virtual protected void AddAdditionalElementData(ref ItemUiElement element, ItemData item) { }
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

	virtual public void OnElementClicked()
	{
		m_selected = UiEventSystem.Get.Selected.GetComponent<ItemUiElement>();
		m_itemGuid = m_selected.ItemGuid;
		m_selectedData = m_itemListManager.GetItem(m_itemGuid);

		RemoveSampleObject();
		m_sampleObject = Instantiate(ItemLookupManager.Get.LookupItem(m_selected.Guid));
		m_sampleObject.GetComponent<SelectableBase>().SetInventoryPreviewState();

		if (m_selectedData.additionalData != null)
		{
			m_selectedData.additionalData.LoadData(m_sampleObject);
		}

		foreach (MeshRenderer renderer in m_sampleObject.GetComponentsInChildren<MeshRenderer>())
		{
			if (renderer.gameObject.layer != 9)
				renderer.gameObject.layer = 11;
		}

		m_sampleObject.transform.position += m_sampleObject.GetComponent<ItemComponent>().Offset;
		m_sampleObject.transform.rotation = Quaternion.Euler(m_sampleObject.GetComponent<ItemComponent>().Rotation);
	}

	protected void CreateMenuElements()
	{
		foreach (GameObject element in m_elements)
		{
			Destroy(element);
		}

		m_elements.Clear();

		foreach (ItemData item in m_itemListManager.Items)
		{
			ItemUiElement element = ((GameObject)Instantiate(m_elementTemplate.gameObject, m_scrollView.content)).GetComponent<ItemUiElement>();
			element.gameObject.SetActive(true);
			element.GetComponentInChildren<Text>().text = item.Name;
			element.Guid = item.Guid;
			element.ItemGuid = item.ItemGuid;
			element.Tags = item.Tags;
			element.Price = item.Price;
			m_elements.Add(element.gameObject);
		}
	}

		void RemoveSampleObject()
	{
		if (m_sampleObject != null)
		{
			Destroy(m_sampleObject);
			m_sampleObject = null;
		}
	}

	public void LateUpdate()
	{
		if (StateManager.Get.State != GetState())
			return;

		if (InputManager.Get.IsJustPressed(EActions.Select) && !UiEventSystem.Get.IsMouseOverUi)
			StateManager.Get.TrySetState(EGameState.Viewing);

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving) && !UiEventSystem.Get.IsMouseOverUi)
			StateManager.Get.TrySetState(EGameState.CameraMoving);
	}
}


