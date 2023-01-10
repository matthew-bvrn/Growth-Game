using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableComponent : MonoBehaviour
{
	public enum EType
	{
		Item,
		Leaf,
		Baby
	}

	public EType Type = EType.Item;

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
}
