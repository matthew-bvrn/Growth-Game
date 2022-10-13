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

	public ItemData GetItem(string itemGuid)
	{
		return Items.Find((x) => x.ItemGuid == itemGuid);
	}

	public void PlaceItem(string itemGuid)
	{
		ItemData itemData = Get.GetItem(itemGuid);

		GameObject newObject = Instantiate(ItemLookupManager.Get.LookupItem(itemData.Guid));

		if (itemData.additionalData != null)
		{
			itemData.additionalData.LoadData(newObject);
		}

		newObject.transform.position = new Vector3(0, 10000, 0);
		SelectablesManager.Get.SetObjectMovingState(newObject.GetComponent<SelectableBase>());
		Items.Remove(Items.Find(elem => elem.Guid == itemData.Guid));
	}
}
