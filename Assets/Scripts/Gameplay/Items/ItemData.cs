using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdditionalData 
{
	public abstract void LoadData(GameObject gameObject);
}

public class ItemData
{
	public ItemData(string name, string guid, string itemGuid, List<string> _tags, int _price) { Name = name; Guid = guid; ItemGuid = itemGuid; Tags = _tags; Price = _price; }

	public string Name { get; private set; }
	public List<string> Tags { get; private set; }
	public string Guid { get; private set; }
	public string ItemGuid { get; private set; }
	public int Price { get; private set; }
	public AdditionalData additionalData;
}