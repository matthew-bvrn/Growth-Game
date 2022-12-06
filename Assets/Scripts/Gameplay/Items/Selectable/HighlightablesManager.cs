using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public delegate void OnSelectedEvent(GameObject selectable);

public class HighlightablesManager : MonoBehaviour
{
	public static HighlightablesManager Get;

	HighlightableComponent m_highlighted;

	public event OnSelectedEvent SelectedEvent;

	public bool CanSelect = true;

	void Start()
	{
		if (Get == null)
		{
			Get = this;
			SelectedEvent += SelectablesManager.Get.OnSelected;
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

	public void Update()
	{
		if (StateManager.Get.StateChanged)
			return;

		if (StateManager.Get.State == EGameState.Viewing || StateManager.Get.State == EGameState.ObjectSelected)
		{
			HighlightItems(HighlightableComponent.EType.Item);
		}
		else if (StateManager.Get.State == EGameState.Pruning)
		{
			HighlightItems(HighlightableComponent.EType.Leaf);
		}
		else
		{
			TryHighlight(null, true);
		}
	}

	void HighlightItems(HighlightableComponent.EType type)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(InputManager.Get.GetSelectionPosition());
		if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Selectable")))
		{
			HighlightableComponent component = hit.transform.GetComponent<HighlightableComponent>();
			if(component.Type == type)
				TryHighlight(hit.transform.GetComponent<HighlightableComponent>(), true);
		}
		else
			TryHighlight(null, true);

		if (m_highlighted)
			if (InputManager.Get.IsJustPressed(EActions.Select) && CanSelect)
				SelectedEvent.Invoke(m_highlighted.transform.parent.gameObject);
	}
}
