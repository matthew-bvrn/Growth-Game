using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemLookupManager : MonoBehaviour
{
	Dictionary<string, GameObject> m_objects = new Dictionary<string, GameObject>();
	[SerializeField] List<ItemComponent> m_items;

	public void Start()
	{
		foreach(ItemComponent item in m_items)
		{
			m_objects.Add(item.Guid, item.gameObject);
		}
	}
}
