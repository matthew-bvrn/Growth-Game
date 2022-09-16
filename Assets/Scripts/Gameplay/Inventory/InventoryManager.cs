using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	List<ItemData> m_items = new List<ItemData>();

	public static InventoryManager Get;

	void Start()
	{
		if (Get == null)
			Get = this;
		else
			Destroy(this);
	}

	public void MoveToInventory(ItemComponent item)
	{
		m_items.Add(item.GetItemData());
		Destroy(item.gameObject);
	}
}
