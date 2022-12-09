using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ItemListManager : MonoBehaviour
{
	public List<ItemData> Items { get; protected set; } = new List<ItemData>();

	public ItemData GetItem(string itemGuid)
	{
		return Items.Find((x) => x.ItemGuid == itemGuid);
	}
}
