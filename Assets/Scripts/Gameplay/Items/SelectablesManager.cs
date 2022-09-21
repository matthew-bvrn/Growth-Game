using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectablesManager : MonoBehaviour
{
	public static SelectablesManager Get;

	Dictionary<Vector3Int, HighlightableComponent> m_selectables = new Dictionary<Vector3Int, HighlightableComponent>();

	HighlightableComponent m_highlighted;
	public SelectableBase Selected { get; private set; }

	[SerializeField] Camera m_selectableCamera;
	RenderTexture m_rt;
	Texture2D m_tex;

	void Start()
	{
		if (Get == null)
		{
			Get = this;
			int resWidth = Screen.width;
			int resHeight = Screen.height;
			m_rt = new RenderTexture(resWidth, resHeight, 24);
			m_tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
		}
		else
		{
			Destroy(this);
		}
	}

	Vector3Int Convert(Color colour)
	{
		return new Vector3Int(Mathf.RoundToInt(colour.r * 10), Mathf.RoundToInt(colour.g * 10), Mathf.RoundToInt(colour.b * 10));
	}

	public void Register(Color colour, HighlightableComponent selectable)
	{
		m_selectables.Add(Convert(colour), selectable);
	}

	public void Unregister(Color colour)
	{
		m_selectables.Remove(Convert(colour));
	}

	HighlightableComponent GetSelectable(Color colour)
	{
		if (m_selectables.ContainsKey(Convert(colour)))
			return m_selectables[Convert(colour)];

		return null;
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
		Vector2 pos = InputManager.Get.GetSelectionPosition();

		var view = m_selectableCamera.ScreenToViewportPoint(pos);
		var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

		if (isOutside)
			return;

		if (StateManager.Get.State != EGameState.Viewing && StateManager.Get.State != EGameState.ObjectSelected)
		{
			TryHighlight(null, true);
			return;
		}

		//RenderTexture currentActiveRT = RenderTexture.active; 
		m_selectableCamera.targetTexture = m_rt;

		m_selectableCamera.Render();

		RenderTexture.active = m_rt;
		m_tex.ReadPixels(new Rect((int)pos.x, Screen.height - (int)pos.y, 1, 1), 0, 0, false);

		m_selectableCamera.targetTexture = null;
		//RenderTexture.active = currentActiveRT;
		Color colour = m_tex.GetPixel(0, 0);

		TryHighlight(GetSelectable(colour), true);

		if (InputManager.Get.IsJustPressed(EActions.Select) && !HighlightSystem.Get.ElementHighlighted)
		{
			if (m_highlighted != null) //select highlighted object
			{
				if (m_highlighted.transform.parent.GetComponent<SelectableBase>() != null)
				{
					Selected = m_highlighted.transform.parent.GetComponent<SelectableBase>();
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
				StateManager.Get.TrySetState(EGameState.Viewing);
			}
		}

		if(InputManager.Get.IsJustPressed(EActions.CameraMoving))
		{
			StateManager.Get.TrySetState(EGameState.CameraMoving);
		}
	}
}
