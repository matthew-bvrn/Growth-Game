using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
	public ItemData(string name, string guid) { Name = name; Guid = guid; }

	public string Name { get; private set; }
	public string Guid { get; private set; }
}
