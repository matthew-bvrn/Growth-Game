using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableWallMounted : SelectableObject
{
	bool IsHitValid(RaycastHit hit)
	{
		if (hit.transform.gameObject.tag != "Wall")
			return false;

		//Make sure sufficiently much of the surface is showing
		float dot = Vector3.Dot(hit.transform.up, Camera.main.transform.forward);
		return dot < 0.3;
	}

	protected override void UpdateObject(RaycastHit[] hits)
	{
		m_canPlace = false;

		foreach (RaycastHit hit in hits)
			if (IsHitValid(hit))
			{
				gameObject.transform.position = hit.point;
				gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, hit.transform.rotation.eulerAngles.y + 90, gameObject.transform.rotation.z);

				m_canPlace = true;
				break;
			}

		if (m_collisions != 0)
			m_canPlace = false;
	}

	internal override void CollisionEnter(Collider collider)
	{
		if (collider.gameObject.tag != "Wall")
			++m_collisions;
	}

	internal override void CollisionExit(Collider collider)
	{
		if (collider.gameObject.tag != "Wall")
			--m_collisions;
	}
}
