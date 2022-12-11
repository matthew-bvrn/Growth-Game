using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : ItemListManager
{
	public static InventoryManager Get;

	public int Money { get; set; } = 9999;

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
