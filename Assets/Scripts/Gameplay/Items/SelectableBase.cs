using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESelectableState
{
	Default,
	Moving
}

public abstract class SelectableBase : MonoBehaviour
{
	[SerializeField] protected GameObject m_model;

	internal ESelectableState State { get; set; }

	protected bool m_canPlace = false;

	Vector3 m_position;
	Quaternion m_rotation;

	protected abstract void UpdateObject(RaycastHit[] hits);
	protected abstract bool CollisionValid(Collider collider);

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	void OnStateChanged(EGameState state)
	{
		if(state == EGameState.ObjectMoving && SelectablesManager.Get.Selected == this)
		{
			m_position = transform.position;
			m_rotation = transform.rotation;
		}
	}

void Update()
	{
		if (State == ESelectableState.Moving)
		{
			m_canPlace = false;

			Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
			RaycastHit[] placeHits = Physics.RaycastAll(ray, 100, ~LayerMask.GetMask("Object", "Selectable"));

			if (placeHits.Length != 0)
				UpdateObject(placeHits);

			if (!m_canPlace && m_model.activeInHierarchy)
				m_model.SetActive(false);
			else if (m_canPlace && !m_model.activeInHierarchy)
				m_model.SetActive(true);

			if (InputManager.Get.IsJustPressed(EActions.PlaceObject) && m_canPlace)
			{
				State = ESelectableState.Default;
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
					State = ESelectableState.Default;
					transform.position = m_position;
					transform.rotation = m_rotation;
					StateManager.Get.TrySetState(EGameState.Viewing);
				}
			}
		}
	}
}

