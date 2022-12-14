using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemLookupManager : MonoBehaviour
{
	public static ItemLookupManager Get;

	Dictionary<string, GameObject> m_objects = new Dictionary<string, GameObject>();
	[SerializeField] List<ItemComponent> m_items;

	public void Start()
	{
		if(Get == null)
		{
			Get = this;
		}
		else
		{
			Destroy(this);
			return;
		}

		foreach(ItemComponent item in m_items)
		{
			m_objects.Add(item.Guid, item.gameObject);
		}
	}

	public GameObject LookupItem(string guid)
	{
		GameObject gameObject = null;
		m_objects.TryGetValue(guid, out gameObject);
		return gameObject;
	}

	public ItemData LoadItemDataFromFile(string filename)
	{
		string data = Resources.Load("Data/" + filename).ToString();
		return JsonConvert.DeserializeObject<ItemData>(data, new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All
		});
	}
}
