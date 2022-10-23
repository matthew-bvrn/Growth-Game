using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableWallMounted : SelectableBase
{
	protected override bool IsHitValid(RaycastHit hit)
	{
		if (!TagIsPlaceableSurface(hit.transform.gameObject.tag))
			return false;

		//Make sure sufficiently much of the surface is showing
		float dot = Vector3.Dot(hit.transform.up, Camera.main.transform.forward);
		return dot < 0.3;
	}

	protected override bool TagIsPlaceableSurface(string tag)
	{
		return tag == "Wall";
	}

	protected override void OnStateChangedInternal()
	{
		//TODO
	}

	protected override bool CheckOnSurface(Vector3 position)
	{
		return position.y < 5 && position.y > 0;
	}

	protected override void UpdateObject(RaycastHit[] hits)
	{
		foreach (RaycastHit hit in hits)
		{
			if (IsHitValid(hit))
			{
				gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, hit.transform.rotation.eulerAngles.y + 90, gameObject.transform.rotation.z);
				break;
			}
		}

		Vector3 right = gameObject.transform.right;

		Vector3 cross = Vector3.Cross(right, Vector3.up);

		FindPlacePoint(hits, false, new List<Vector3> { Vector3.up, Vector3.down, cross, -cross }, delegate (Vector3 pos) { gameObject.transform.position = pos; });
	}
}
