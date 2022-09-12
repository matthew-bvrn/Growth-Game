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

	void Start()
	{
		if (Get == null)
		{
			Get = this;
			int resWidth = Screen.width;
			int resHeight = Screen.height;
			m_rt = new RenderTexture(resWidth, resHeight, 24);
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
		if (StateManager.Get.State != GameState.Viewing)
		{
			TryHighlight(null, true);
			return;
		}

		//RenderTexture currentActiveRT = RenderTexture.active; 
		m_selectableCamera.targetTexture = m_rt;

		m_selectableCamera.Render();

		RenderTexture.active = m_rt;
		Texture2D tex = new Texture2D(m_rt.width, m_rt.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

		m_selectableCamera.targetTexture = null;
		//RenderTexture.active = currentActiveRT;

		Vector2 pos = InputManager.Get.GetSelectionPosition();
		Color colour = tex.GetPixel((int)pos.x, (int)pos.y);

		TryHighlight(GetSelectable(colour), false);
	}
}
