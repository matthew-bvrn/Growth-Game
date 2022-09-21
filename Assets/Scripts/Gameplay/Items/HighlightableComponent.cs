using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableComponent : MonoBehaviour
{
	int m_id;
	Color m_colour;

	internal bool Highlighted
	{
		get
		{
			return m_selected;
		}
		set
		{
			m_selected = value;

			foreach (Renderer child in transform.parent.gameObject.GetComponentsInChildren<Renderer>())
			{
				if (child.GetComponent<HighlightableComponent>() != null)
					continue;

				if (m_selected)
					child.renderingLayerMask = 2;
				else
					child.renderingLayerMask = 1;
			}
		}
	}

	bool m_selected;

	static int m_idCount = 0;

	// Start is called before the first frame update
	void Start()
	{
		m_id = ++m_idCount;
		m_colour = new Color((m_id % 10) / 10f, ((m_id / 10) % 10) / 10f, ((m_id / 100) % 10) / 10f);
		SelectablesManager.Get.Register(m_colour, this);

		if (GetComponent<Renderer>() == null)
		{
			Debug.LogWarning(gameObject.name + "has a selectable component but no renderer, it will be skipped.");
			return;
		}

		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
		propertyBlock.SetColor("ObjectColor", m_colour);
		GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
	}

	void OnDestroy()
	{
		SelectablesManager.Get.Unregister(m_colour);
	}
}
