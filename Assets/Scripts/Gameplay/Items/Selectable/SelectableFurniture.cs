using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableFreestanding
{
	protected override bool TagIsPlaceableSurface(string tag)
	{
		return tag == "Floor";
	}
}
