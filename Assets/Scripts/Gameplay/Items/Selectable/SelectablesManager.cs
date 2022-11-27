using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public delegate void OnSelectedEvent(SelectableBase selectable);

public class SelectablesManager : MonoBehaviour
{
	public static SelectablesManager Get;

	HighlightableComponent m_highlighted;
	public SelectableBase Selected { get; private set; }
	public event OnSelectedEvent OnSelected;

	void Start()
	{
		if (Get == null)
		{
			Get = this;
		}
		else
		{
			Destroy(this);
		}
	}

	bool TryHighlight(HighlightableComponent selectable, bool forceIfNull)
	{
		if (m_highlighted != null)
			m_highlighted.Highlighted = false;

		if (selectable == null && !forceIfNull)
			return false;

		m_highlighted = selectable;
		if (m_highlighted != null)
			m_highlighted.Highlighted = true;

		return true;
	}

	public void SetObjectMovingState(SelectableBase selectable = null)
	{
		if(selectable!=null)
		{
			Selected = selectable;
		}

		Selected.State = ESelectableState.Moving;
		StateManager.Get.TrySetState(EGameState.ObjectMoving);
	}

	public void Update()
	{
		if (StateManager.Get.State != EGameState.Viewing && StateManager.Get.State != EGameState.ObjectSelected)
		{
			TryHighlight(null, true);
			return;
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
		if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Selectable")))
			TryHighlight(hit.transform.GetComponent<HighlightableComponent>(), true);
		else
			TryHighlight(null, true);


		if (InputManager.Get.IsJustPressed(EActions.Select) && !UiEventSystem.Get.ElementHighlighted)
		{
			if (m_highlighted != null) //select highlighted object
			{
				if (m_highlighted.transform.parent.GetComponent<SelectableBase>() != null)
				{
					Selected = m_highlighted.transform.parent.GetComponent<SelectableBase>();
					OnSelected.Invoke(Selected);
					StateManager.Get.TrySetState(EGameState.ObjectSelected);
				}
				else
				{
					Debug.LogError("Tried to select the parent of " + m_highlighted.name + ", but the parent is not a selectable object.");
				}
			}
			else //unselect
			{
				Selected = null;
				OnSelected.Invoke(Selected);
				StateManager.Get.TrySetState(EGameState.Viewing);
			}
		}

		if(InputManager.Get.IsJustPressed(EActions.CameraMoving))
		{
			StateManager.Get.TrySetState(EGameState.CameraMoving);
		}
	}
}
