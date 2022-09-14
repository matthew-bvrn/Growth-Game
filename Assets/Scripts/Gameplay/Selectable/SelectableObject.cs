using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESelectableState
{
	Default,
	Moving
}

public abstract class SelectableObject : MonoBehaviour
{
	internal ESelectableState State { get; set; }

	protected bool m_canPlace = false;
	protected Collider m_collider;

	Vector3 m_position;
	Quaternion m_rotation;

	protected abstract void UpdateObject(RaycastHit[] placeHits, RaycastHit[] collisionHits);
	protected abstract bool CollisionValid(Collider collider);

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	internal void CollisionEnter(Collider collider)
	{
		if(CollisionValid(collider))
		{
			m_collider = collider;
		}
	}

	internal void CollisionExit(Collider collider)
	{
		if (CollisionValid(collider))
		{
			//m_collider = null;
		}
	}


	void OnStateChanged(EGameState state)
	{
		if(state == EGameState.ObjectMoving)
		{
			m_position = transform.position;
			m_rotation = transform.rotation;
		}
	}

void Update()
	{
		if (State == ESelectableState.Moving)
		{
			Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
			RaycastHit[] placeHits = Physics.RaycastAll(ray, 100, ~LayerMask.GetMask("Object", "Selectable"));
			RaycastHit[] collisionHits = Physics.RaycastAll(ray, 100);

			if (placeHits.Length != 0)
			{
				UpdateObject(placeHits, collisionHits);

				GetComponentInChildren<Collider>();

				if (InputManager.Get.IsJustPressed(EActions.PlaceObject) && m_canPlace)
				{
					State = ESelectableState.Default;
					StateManager.Get.TrySetState(EGameState.Viewing);
				}
			}

			if(InputManager.Get.IsJustPressed(EActions.CancelMoveObject))
			{
				State = ESelectableState.Default;
				transform.position = m_position;
				transform.rotation = m_rotation;
				StateManager.Get.TrySetState(EGameState.Viewing);
			}
		}
	}
}

