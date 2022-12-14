using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemsManager : ItemListManager
{
	public static ShopItemsManager Get;

	void Start()
	{
		if (Get != null)
		{
			Destroy(this);
			return;
		}

		Get = this;

		Items.Add(ItemLookupManager.Get.LookupItem("d7420f2d-ec4d-4144-ac36-bc10951e5c5f").GetComponent<ItemComponent>().GetItemData());
		Items.Add(ItemLookupManager.Get.LoadItemDataFromFile("c371a385-8e1a-4b7f-ab13-f701c5382dfb"));
	}

}
