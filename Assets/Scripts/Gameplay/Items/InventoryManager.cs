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

	public void PlaceItem(string guid)
	{
		GameObject newObject = Instantiate(ItemLookupManager.Get.LookupItem(guid));
		newObject.transform.position = new Vector3(0, 10000, 0);
		SelectablesManager.Get.SetObjectMovingState(newObject.GetComponent<SelectableBase>());
		Items.Remove(Items.Find(elem => elem.Guid == guid));
	}
}
