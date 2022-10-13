using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdditionalData 
{
	public abstract void LoadData(GameObject gameObject);
}

public class ItemData
{
	public ItemData(string name, string guid, string itemGuid) { Name = name; Guid = guid; ItemGuid = itemGuid; }

	public string Name { get; private set; }
	public string Guid { get; private set; }
	public string ItemGuid { get; private set; }
	public AdditionalData additionalData;
}