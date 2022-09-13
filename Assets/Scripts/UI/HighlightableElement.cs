using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HighlightableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData data)
	{
		HighlightSystem.Get.AddHighlighted();
	}

	public void OnPointerExit(PointerEventData data)
	{
		HighlightSystem.Get.RemoveHighlighted();
	}

	private void OnDisable()
	{
		HighlightSystem.Get.RemoveHighlighted();
	}
}
