using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;
	[SerializeField] RectTransform m_elementTemplate;

	List<GameObject> m_elements = new List<GameObject>();

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
			m_elements.Add(element);
		}
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
