using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablesManager : MonoBehaviour
{
	public static SelectablesManager Get;

	Dictionary<Vector3Int, Selectable> m_selectables = new Dictionary<Vector3Int, Selectable>();

	public Selectable Selected { get; private set; }

	[SerializeField] Camera m_selectableCamera;

	void Start()
	{
		if(Get == null)
		{
			Get = this;
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

	public void Update()
	{
		if (StateManager.Get.State != GameState.Viewing && StateManager.Get.State != GameState.ObjectHighlighted)
			return;

		if (InputManager.Get.IsJustPressed(EActions.SelectObject))
		{
			int resWidth = Screen.width;
			int resHeight = Screen.height;

			RenderTexture currentActiveRT = RenderTexture.active;

			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			m_selectableCamera.targetTexture = rt;

			m_selectableCamera.Render();

			RenderTexture.active = rt;
			Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
			tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

			m_selectableCamera.targetTexture = null;
			RenderTexture.active = currentActiveRT;
			Destroy(rt);

			Vector2 pos = InputManager.Get.GetSelectionPosition();
			Color colour = tex.GetPixel((int)pos.x, (int)pos.y);

			Selected = GetSelectable(colour);
			if (Selected != null)
				StateManager.Get.TrySetState(GameState.ObjectHighlighted);
			else
				StateManager.Get.TrySetState(GameState.Viewing);
		}

	}
}
