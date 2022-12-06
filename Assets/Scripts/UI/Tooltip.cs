using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
	public Object m_prefab;
	public string m_text;
	GameObject m_tooltip;

	// Start is called before the first frame update
	void Start()
	{
		//m_prefab = Resources.Load("Prefabs/Ui/Tooltip.prefab");
		GetComponent<UiButton>().m_onHighlighted.AddListener(OnHighlighted);
		GetComponent<UiButton>().m_onUnhighlighted.AddListener(OnUnhighlighted);
	}

	void OnHighlighted()
	{
		m_tooltip = (GameObject)Instantiate(m_prefab, transform.parent);
		m_tooltip.GetComponentInChildren<UiButton>().RestartAnimation();
		m_tooltip.GetComponentInChildren<TMP_Text>().text = m_text;
	}

	private void Update()
	{
		if (m_tooltip)
		{
			m_tooltip.transform.position = Input.mousePosition;
		}
	}

	void OnUnhighlighted()
	{
		Destroy(m_tooltip);
	}

	private void OnDisable()
	{
		Destroy(m_tooltip);
	}
}
