using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESelectableState
{
	InventoryPreview,
	Placed,
	Moving
}

public abstract class SelectableBase : MonoBehaviour
{
	public ESelectableState State { get; internal set; }

	protected bool m_canPlace = false;
	bool m_visible = true;

	Vector3 m_position;
	Quaternion m_rotation;

	protected abstract void UpdateObject(RaycastHit[] hits);
	protected abstract void OnStateChangedInternal();

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

			if (!m_canPlace && m_visible)
				SetIsVisible(false);
			else if (m_canPlace && !m_visible)
				SetIsVisible(true);

			if (InputManager.Get.IsJustPressed(EActions.PlaceObject) && m_canPlace)
			{
				CheckSurface();

				State = ESelectableState.Placed;
				StateManager.Get.TrySetState(EGameState.Viewing);
			}

			if (InputManager.Get.IsJustPressed(EActions.CancelMoveObject))
			{
				SetIsVisible(true);

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

	void SetIsVisible(bool visible)
	{
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
			renderer.enabled = visible;
		m_visible = visible;
	}
}

