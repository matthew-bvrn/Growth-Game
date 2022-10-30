using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableFreestanding
{
	protected override bool IsHitValid(RaycastHit hit)
	{
		return hit.transform.gameObject.tag == "Floor";
	}
}
