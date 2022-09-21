using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public class SelectableFurniture : SelectableBase
{
	List<Vector3> m_checkedPoints = new List<Vector3>();
	bool m_wasRotated = false;

	protected override void UpdateObject(RaycastHit[] hits)
	{
		m_wasRotated = false;

		float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

		if (rotate < 0)
		{
			gameObject.transform.Rotate(new Vector3(0, 15, 0));
			m_wasRotated = true;
		}
		if (rotate > 0)
		{
			gameObject.transform.Rotate(new Vector3(0, -15, 0));
			m_wasRotated = true;
		}

		Vector3 hitPoint = new Vector3();

		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.gameObject.tag == "Floor")
			{
				m_canPlace = true;
				hitPoint = hit.point;
			}
		}

		Collider collider = GetComponentInChildren<Collider>();
		Queue<Vector3> positions = new Queue<Vector3>();
		m_checkedPoints.Clear();
		m_checkedPoints.Add(hitPoint);
		positions.Enqueue(hitPoint);
		Vector3 position;
		

		if(!RecursiveFindPoint(collider, positions, out position))
		{
			m_canPlace = false;
			return;
		}

		if(position != hitPoint)
		{
			while (true)
			{
				position -= 0.01f * (position - hitPoint).normalized;
				if (CheckCollision(collider, position))
					break;
			}

			if (Vector3.Distance(hitPoint, position) < Vector3.Distance(gameObject.transform.position, hitPoint) || m_wasRotated)
			{
				gameObject.transform.position = position;
			}
		}
		else
		{
			gameObject.transform.position = position;
		}
	}

	bool RecursiveFindPoint(Collider collider, Queue<Vector3> positions, out Vector3 pos)
	{
		pos = new Vector3();
		List<Vector3> offsets = new List<Vector3> { new Vector3(0.5f, 0, 0), new Vector3(-0.5f, 0, 0), new Vector3(0, 0, 0.5f), new Vector3(0, 0, -0.5f) };

		while (positions.Count > 0)
		{
			pos = positions.Dequeue();

			if (Mathf.Abs(pos.x) > 5.5 || Mathf.Abs(pos.z) > 5.5)
				continue;

			if (!CheckCollision(collider, pos))
				return true;

			foreach (Vector3 offset in offsets)
			{
				if (!m_checkedPoints.Contains(pos + offset))
				{
					m_checkedPoints.Add(pos + offset);
					positions.Enqueue(pos + offset);
				}
			}
		}
		return false;
	}

	bool CheckCollision(Collider inCollider, Vector3 position)
	{
		Collider[] colliders;
		if (inCollider.GetType() == typeof(BoxCollider))
		{
			BoxCollider boxCollider = (BoxCollider)inCollider;
			colliders = Physics.OverlapBox(position, boxCollider.transform.localScale / 2, boxCollider.transform.rotation);
		}
		else
		{
			CapsuleCollider capsuleCollider = (CapsuleCollider)inCollider;
			colliders = Physics.OverlapCapsule(position + new Vector3(0, capsuleCollider.height / 2, 0), position - new Vector3(0, capsuleCollider.height / 2, 0), capsuleCollider.radius);
		}

		foreach (Collider collider in colliders)
		{
			if (collider.gameObject.tag != "Floor" && collider != inCollider)
			{
				return true;
			}
		}
		return false;
	}

	protected override bool CollisionValid(Collider collider)
	{
		return collider.gameObject.tag != "Floor";
	}
}
