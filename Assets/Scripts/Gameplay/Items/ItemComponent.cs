using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Placeholder for UniqueIdDrawer script
public class UniqueIdentifierAttribute : PropertyAttribute { }

public class ItemComponent : MonoBehaviour
{
	[SerializeField] string m_name;
	[UniqueIdentifier] public string Guid;
	public Vector3 Offset;
	public Vector3 Rotation;

	public ItemData GetItemData()
	{
		ItemData itemData = new ItemData(m_name, Guid);

		AdditionalDataFetcher fetcher = GetComponentInChildren<AdditionalDataFetcher>();

		if(fetcher!=null)
		{
			itemData.additionalData = fetcher.GetData();
		}

		return itemData;
	}
}
