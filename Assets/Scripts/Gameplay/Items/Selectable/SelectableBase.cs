using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ESelectableState
{
	InventoryPreview,
	Placed,
	Moving
}

public abstract class SelectableBase : MonoBehaviour
{
	[SerializeField] protected GameObject m_model;

	public ESelectableState State { get; internal set; }

	Vector3 m_position;
	Quaternion m_rotation;

	protected Vector3 m_workingPosition;

	protected List<Vector3> m_checkedPoints = new List<Vector3>();
	protected bool m_canPlace = false;	

	protected abstract void UpdateObject(RaycastHit[] hits);
	protected abstract void OnStateChangedInternal();
	protected abstract bool IsHitValid(RaycastHit hit);
	protected abstract bool TagIsPlaceableSurface(string tag);
	protected virtual bool CheckOnSurface(Vector3 position)
	{
		return true;
	}

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	public void SetInventoryPreviewState()
	{
		State = ESelectableState.InventoryPreview;
	}

	void OnStateChanged(EGameState state)
	{
		if (state == EGameState.ObjectMoving && SelectablesManager.Get.Selected == this)
		{
			m_position = transform.position;
			m_rotation = transform.rotation;
			OnStateChangedInternal();
		}
	}

	void Update()
	{
		if (State == ESelectableState.Moving)
		{
			m_canPlace = false;

			Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
			RaycastHit[] placeHits = Physics.RaycastAll(ray, 100, ~LayerMask.GetMask("Selectable"));

			if (placeHits.Length != 0)
				UpdateObject(placeHits);

			if (!m_canPlace && m_model.activeInHierarchy)
				m_model.SetActive(false);
			else if (m_canPlace && !m_model.activeInHierarchy)
				m_model.SetActive(true);

			if (InputManager.Get.IsJustPressed(EActions.PlaceObject) && m_canPlace)
			{
				CheckSurface();

				State = ESelectableState.Placed;
				StateManager.Get.TrySetState(EGameState.Viewing);
			}

			if (InputManager.Get.IsJustPressed(EActions.CancelMoveObject))
			{
				m_model.SetActive(true);

				if (StateManager.Get.PreviousState == EGameState.InventoryOpen)
				{
					InventoryManager.Get.MoveToInventory(GetComponent<ItemComponent>());
					StateManager.Get.TrySetState(EGameState.InventoryOpen);
				}
				else
				{
					State = ESelectableState.Placed;
					transform.position = m_position;
					transform.rotation = m_rotation;
					StateManager.Get.TrySetState(EGameState.Viewing);
				}
			}
		}
	}

	void CheckSurface()
	{
		Ray ray = new Ray(transform.position+new Vector3(0,0.1f,0), Vector3.down);
		RaycastHit[] hits = Physics.RaycastAll(ray, 0.25f);
		foreach(RaycastHit hit in hits)
		{
			if(hit.transform.tag == "Surface")
			{
				hit.transform.parent.GetComponent<SurfaceComponent>().OnSurfaceDestroyed += () => 
				{
					transform.parent = null;
					transform.position = Physics.RaycastAll(transform.position, Vector3.down)[0].point;
					foreach(Behaviour component in GetComponents(typeof(Behaviour)))
					{
						component.enabled = true;
					}
				};
				transform.parent = hit.transform.parent;
				return;
			}
		}

		transform.parent = null;
	}
	
	protected void FindPlacePoint(RaycastHit[] hits, bool wasRotated, List<Vector3> offsets, Action<Vector3> callback)
	{
		Vector3 hitPoint = new Vector3();
		float distance = float.PositiveInfinity;

		foreach (RaycastHit hit in hits)
		{
			if (IsHitValid(hit))
			{
				m_canPlace = true;
				if (hit.distance < distance)
				{
					hitPoint = hit.point;
					distance = hit.distance;
				}
			}
		}

		Collider collider = GetComponentInChildren<Collider>();
		Queue<Vector3> positions = new Queue<Vector3>();
		m_checkedPoints.Clear();
		m_checkedPoints.Add(hitPoint);
		positions.Enqueue(hitPoint);
		Vector3 position;

		if (!RecursiveFindPoint(collider, positions, out position, offsets))
		{
			//no position found
			if (m_workingPosition == new Vector3())
			{
				m_canPlace = false;
				return;
			}
			gameObject.transform.position = m_workingPosition;
		}

		//if it cant be placed exactly at the hit point
		else if (position != hitPoint)
		{
			while (true)
			{
				position -= 0.01f * (position - hitPoint).normalized;
				if (CheckCollision(collider, position))
					break;
			}

			if (Vector3.Distance(hitPoint, position) < Vector3.Distance(gameObject.transform.position, hitPoint) || wasRotated)
			{
				gameObject.transform.position = position;
				m_workingPosition = gameObject.transform.position;
			}
		}
		else
		{
			//if it can be placed at the hit point, then just place it there
			gameObject.transform.position = position;
			m_workingPosition = gameObject.transform.position;
		}
	}
	
	bool RecursiveFindPoint(Collider collider, Queue<Vector3> positions, out Vector3 pos, List<Vector3> offsets)
	{
		pos = new Vector3();

		float interval = 0.5f;

		for(int i = 0; i<offsets.Count; i++)
			offsets[i] *= interval;

		while (positions.Count > 0)
		{
			pos = positions.Dequeue();

			if (Mathf.Abs(pos.x) > 5.1 || Mathf.Abs(pos.z) > 5.1 || Mathf.Abs(pos.y) > 5.1)
				continue;

			if (!CheckCollision(collider, pos))
				if(CheckOnSurface(pos))
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
			float radius = capsuleCollider.transform.localScale.x / 2;
			colliders = Physics.OverlapCapsule(position + new Vector3(0, radius, 0), position + new Vector3(0, capsuleCollider.transform.localScale.y + radius, 0), radius);
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

