using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public List<ItemData> Items { get; private set; } = new List<ItemData>();

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
		Items.Add(item.GetItemData());
		Destroy(item.gameObject);
	}
}
