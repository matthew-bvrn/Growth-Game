using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableObject
{
	protected override bool CanPlace(RaycastHit hit)
	{
		return hit.transform.gameObject.tag == "Floor";
	}
}
