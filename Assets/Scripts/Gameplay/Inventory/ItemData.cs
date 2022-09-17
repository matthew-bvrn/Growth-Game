using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
	public ItemData(string name) { Name = name; }

	public string Name { get; private set; }
}
