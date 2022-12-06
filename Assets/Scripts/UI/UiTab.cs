using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTab : UiButton
{
	public UiTabManager m_tabManager;
	public string m_tag;


	// Start is called before the first frame update
	void Start()
	{
		Type = ButtonType.Selectable;
		m_onSelected.AddListener(OnSelected);
	}

	void OnSelected()
	{
		m_tabManager.ActiveTag = m_tag;
	}

	private void OnEnable()
	{
		m_tabManager.TrySetDefault(this);
	}
}
