using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUiElement : MonoBehaviour
{
	[SerializeField] InventoryMenu m_menu;

	internal List<string> Tags;

	public string Guid { get; set; }
	public string ItemGuid { get; set; }
}
