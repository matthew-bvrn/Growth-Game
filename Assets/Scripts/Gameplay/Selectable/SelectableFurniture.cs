using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public class SelectableFurniture : SelectableObject
{


	private void TriggerExit(Collider other)
	{
		if (other = m_collider)
			m_isIntersecting = false;
	}

	protected override void UpdateObject(RaycastHit[] placeHits, RaycastHit[] collisionHits)
	{
		m_canPlace = false;

		Vector3 floorPos = new Vector3();

		foreach (RaycastHit hit in placeHits)
		{
			if (hit.transform.gameObject.tag == "Floor")
			{
				m_canPlace = true;
				floorPos = hit.point;

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

		if (m_collider != null)
		{
			Vector3 thisColliderOffset = GetComponentInChildren<Collider>().ClosestPointOnBounds(floorPos) + GetComponentInChildren<Collider>().transform.position;
			Debug.Log(thisColliderOffset);
			thisColliderOffset.y = 0;

			gameObject.transform.position = GetClosestPointOutsideCollider(m_collider, floorPos); //(GetClosestPointOutsideCollider(m_collider, floorPos) - m_collider.transform.position).normalized;
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

	Vector3 GetClosestPointOutsideCollider(Collider collider, Vector3 point)
	{
		if (collider.GetType() == typeof(CapsuleCollider))
		{
			Vector3 centre = collider.transform.parent.position;
			centre.y = 0;

			point = new Vector3(point.x, 0, point.z);

			Vector3 dir = (point - centre).normalized;
			return centre + ((CapsuleCollider)collider).radius * dir;
		}
		else if (collider.GetType() == typeof(BoxCollider))
		{
			Vector3 centre = collider.transform.parent.position;
			centre.y = 0;

			point = new Vector3(point.x, 0, point.z);

			Vector3 dir;

			if (Mathf.Abs(point.x - centre.x) < Mathf.Abs(point.z - centre.z))
			{
				int sign = point.z - centre.z < 0 ? -1 : 1;
				dir = new Vector3(centre.x + point.x, 0, centre.z + sign*((BoxCollider)collider).transform.localScale.z / 2);
			}
			else
			{
				int sign = point.x - centre.x < 0 ? -1 : 1;
				dir = new Vector3(centre.x + sign*((BoxCollider)collider).transform.localScale.x / 2, 0, centre.z + point.z);
			}

			return dir;
		}

		return new Vector3();
	}
}
