using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiElement : MonoBehaviour
{
	protected bool m_mouseInRegion;


	protected void Update()
	{
		RectTransform rect = GetComponent<RectTransform>();

		if (Input.mousePosition.x > rect.position.x + rect.rect.min.x && Input.mousePosition.x < rect.position.x + rect.rect.max.x
			&& Input.mousePosition.y > rect.position.y + rect.rect.min.y && Input.mousePosition.y < rect.position.y + rect.rect.max.y)
		{
			UiEventSystem.Get.IsMouseOverUi = true;
			m_mouseInRegion = true;
		}
		else
		{
			m_mouseInRegion = false;
		}
	}
}
