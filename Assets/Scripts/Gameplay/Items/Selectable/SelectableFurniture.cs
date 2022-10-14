using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableFreestanding
{
	protected override bool IsHitValid(RaycastHit hit)
	{
		return TagIsPlaceableSurface(hit.transform.gameObject.tag);
	}

	protected override bool TagIsPlaceableSurface(string tag)
	{
		return tag == "Floor";
	}
}
