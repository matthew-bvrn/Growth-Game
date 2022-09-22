using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableSurfaceItem : SelectableFreestanding
{
	protected override bool TagIsPlaceableSurface(string tag)
	{
		return tag == "Floor" || tag == "Surface";
	}
}
