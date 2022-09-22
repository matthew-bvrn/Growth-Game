using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public abstract class SelectableFreestanding : SelectableBase
{
	List<Vector3> m_checkedPoints = new List<Vector3>();
	bool m_wasRotated = false;
	Vector3 m_workingPosition;

	protected abstract bool TagIsPlaceableSurface(string tag);

	protected override void OnStateChangedInternal()
	{
		m_workingPosition = new Vector3();
	}

	protected override void UpdateObject(RaycastHit[] hits)
	{
		m_wasRotated = false;

		float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

		if (rotate < 0)
		{
			gameObject.transform.Rotate(new Vector3(0, 15, 0));
			m_wasRotated = true;
			m_workingPosition = new Vector3();
		}
		if (rotate > 0)
		{
			gameObject.transform.Rotate(new Vector3(0, -15, 0));
			m_wasRotated = true;
			m_workingPosition = new Vector3();
		}

		Vector3 hitPoint = new Vector3();
		float distance = float.PositiveInfinity;

		foreach (RaycastHit hit in hits)
		{
			if (TagIsPlaceableSurface(hit.transform.gameObject.tag))
			{
				m_canPlace = true;
				if(hit.distance < distance)
					hitPoint = hit.point;
			}
		}

		Collider collider = GetComponentInChildren<Collider>();
		Queue<Vector3> positions = new Queue<Vector3>();
		m_checkedPoints.Clear();
		m_checkedPoints.Add(hitPoint);
		positions.Enqueue(hitPoint);
		Vector3 position;


		if (!RecursiveFindPoint(collider, positions, out position))
		{
			if (m_workingPosition == new Vector3())
			{
				m_canPlace = false;
				return;
			}
			gameObject.transform.position = m_workingPosition;
		}

		else if (position != hitPoint)
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
				m_workingPosition = gameObject.transform.position;
			}
		}
		else
		{
			gameObject.transform.position = position;
			m_workingPosition = gameObject.transform.position;
		}
	}

	bool RecursiveFindPoint(Collider collider, Queue<Vector3> positions, out Vector3 pos)
	{
		pos = new Vector3();

		float interval = 0.5f;

		List<Vector3> offsets = new List<Vector3> { new Vector3(interval, 0, 0), new Vector3(-interval, 0, 0), new Vector3(0, 0, interval), new Vector3(0, 0, -interval) };

		while (positions.Count > 0)
		{
			pos = positions.Dequeue();

			if (!CheckCollision(collider, pos))
				return true;

			if (Mathf.Abs(pos.x) > 5.5 || Mathf.Abs(pos.z) > 5.5)
				continue;

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
			float radius = capsuleCollider.transform.localScale.x / 2;
			colliders = Physics.OverlapCapsule(position+new Vector3(0, radius, 0), position + new Vector3(0, capsuleCollider.transform.localScale.y+ radius, 0), radius);
		}

		foreach (Collider collider in colliders)
		{
			if (!TagIsPlaceableSurface(collider.gameObject.tag) && collider != inCollider)
			{
				return true;
			}
		}
		return false;
	}
}
