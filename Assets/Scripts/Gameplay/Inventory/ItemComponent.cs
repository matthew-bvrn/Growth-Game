using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
	[SerializeField] string m_name;

	public ItemData GetItemData()
	{
		return new ItemData(m_name);
	}
}
