using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTabManager : MonoBehaviour
{
	internal string ActiveTag { get { return m_activeTag; } set
	{
		m_activeTag = value;

		foreach(GameObject element in GetComponent<ItemMenu>().m_elements)
			{
				if (m_activeTag == "All")
					element.SetActive(true);
				else
				{
					if (element.GetComponent<ItemUiElement>().Tags.Contains(m_activeTag))
						element.SetActive(true);
					else
						element.SetActive(false);
				}
			}
		} }

	string m_activeTag;

	[SerializeField] UiTab m_defaultTab;
	[SerializeField] GameObject m_scrollContent;

	internal void TrySetDefault(UiTab tab)
	{
		if (tab == m_defaultTab)
			UiEventSystem.Get.Select(tab);
	}
}
