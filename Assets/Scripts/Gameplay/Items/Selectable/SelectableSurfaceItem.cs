using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableSurfaceItem : SelectableFreestanding
{
	protected override bool IsHitValid(RaycastHit hit)
	{
		string tag = hit.transform.gameObject.tag;
		return tag == "Floor" || tag == "Surface";
	}
}
