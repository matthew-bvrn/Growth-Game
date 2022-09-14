using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableObject
{
	protected override void UpdateObject(RaycastHit[] placeHits, RaycastHit[] collisionHits)
	{
		m_canPlace = false;

		Vector3 floorPos = new Vector3();

		Bounds colliderBounds = GetComponentInChildren<Collider>().bounds;

		foreach (RaycastHit hit in placeHits)
		{
			if (hit.transform.gameObject.tag == "Floor")
			{
				m_canPlace = true;
				floorPos = hit.point;

				colliderBounds.center = hit.point;

				//gameObject.transform.position = hit.point;
				/*
				if (m_collider != null)
				{
					Collider thisCollider = GetComponentInChildren<Collider>();

					Vector3 pushDir = (m_collider.bounds.center - hit.point);
					pushDir.y = 0;
					pushDir = pushDir.normalized;

					Bounds otherBounds = new Bounds();
					otherBounds.SetMinMax(new Vector3(m_collider.bounds.min.x, 0, m_collider.bounds.min.z), new Vector3(m_collider.bounds.max.x, 0, m_collider.bounds.max.z));

					Bounds thisBounds = new Bounds();
					thisBounds.SetMinMax(new Vector3(thisCollider.bounds.min.x, 0, thisCollider.bounds.min.z), new Vector3(thisCollider.bounds.max.x, 0, thisCollider.bounds.max.z));

					Vector3 boundsPos = m_collider.ClosestPointOnBounds(hit.point);

					Vector3 offset = Vector3.Distance(thisBounds.center, m_collider.bounds.center) * pushDir;

					Debug.Log("distance:" + m_collider.bounds.SqrDistance(hit.point));
					Debug.Log("pushdir: " + pushDir);
					Debug.Log("bounds: "+boundsPos);
					Debug.Log("offset: "+offset);

					gameObject.transform.position = boundsPos - offset;
					//Vector3 offset = GetComponentInChildren<Collider>().ClosestPointOnBounds(hit.point);
					//gameObject.transform.position += -offset + gameObject.transform.position;
				}
				else
				{

				}
				*/

			}
		}

		if(m_collider!= null)
		{
			while (colliderBounds.Intersects(m_collider.bounds))
			{
				Vector3 pushDir = m_collider.bounds.center - colliderBounds.center;
				pushDir.y = 0;
				pushDir = pushDir.normalized;
				colliderBounds.center -= 0.01f * pushDir;
			}
			gameObject.transform.position =new Vector3(colliderBounds.center.x, gameObject.transform.position.y, colliderBounds.center.z);
		}
		else
		{
			gameObject.transform.position = floorPos;
		}

		float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

		if (rotate < 0)
			gameObject.transform.Rotate(new Vector3(0, 15, 0));
		if (rotate > 0)
			gameObject.transform.Rotate(new Vector3(0, -15, 0));
			
	}

	protected override bool CollisionValid(Collider collider)
	{
		return collider.gameObject.tag != "Floor";
	}
}
