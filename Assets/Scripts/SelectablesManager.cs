using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablesManager : MonoBehaviour
{
	public static SelectablesManager Get;

	Dictionary<Vector3Int, Selectable> m_selectables = new Dictionary<Vector3Int, Selectable>();

	public Selectable Highlighted { get; private set; }

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

	public void Register(Color colour, Selectable selectable)
	{
		m_selectables.Add(Convert(colour), selectable);
	}

	public void Unregister(Color colour)
	{
		m_selectables.Remove(Convert(colour));
	}

	Selectable GetSelectable(Color colour)
	{
		if (m_selectables.ContainsKey(Convert(colour)))
			return m_selectables[Convert(colour)];

		return null;
	}

	bool TryHighlight(Selectable selectable, bool forceIfNull)
	{
		if (Highlighted != null)
			Highlighted.Highlighted = false;

		if (selectable == null && !forceIfNull)
			return false;

		Highlighted = selectable;
		if (Highlighted != null)
			Highlighted.Highlighted = true;

		return true;
	}

	public void Update()
	{
		Vector2 pos = InputManager.Get.GetSelectionPosition();

		var view = m_selectableCamera.ScreenToViewportPoint(pos);
		var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;

		if (isOutside)
			return;

		if (StateManager.Get.State != EGameState.Viewing)
		{
			TryHighlight(null, true);
			return;
		}

		//RenderTexture currentActiveRT = RenderTexture.active; 
		m_selectableCamera.targetTexture = m_rt;

		m_selectableCamera.Render();

		RenderTexture.active = m_rt;
		m_tex.ReadPixels(new Rect((int)pos.x, Screen.height-(int)pos.y, 1,1), 0,0, false);

		m_selectableCamera.targetTexture = null;
		//RenderTexture.active = currentActiveRT;
		Color colour = m_tex.GetPixel(0, 0);

		TryHighlight(GetSelectable(colour), false);
	}
}
