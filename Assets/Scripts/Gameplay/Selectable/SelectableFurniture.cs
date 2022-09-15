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

		Vector3 floorPos = new Vector3(0,0,0);

		foreach (RaycastHit hit in placeHits)
		{
			if (hit.transform.gameObject.tag == "Floor")
			{
				m_canPlace = true;
				floorPos = hit.point;
			}
		}

		if(floorPos == new Vector3(0,0,0))
		{
			GetComponentInChildren<Renderer>().enabled = false;
		}
		else
		{
			GetComponentInChildren<Renderer>().enabled = true;
		}
		
		GetComponent<NavMeshAgent>().destination = floorPos;

		if(Vector3.Distance(transform.position, floorPos) < 0.01)
		{
			GetComponent<NavMeshAgent>().isStopped = true;
		}
		else
		{
			GetComponent<NavMeshAgent>().isStopped = false;
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

