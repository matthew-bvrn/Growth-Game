using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableFurniture : SelectableObject
{
	protected override void UpdateObject(RaycastHit[] hits)
	{
		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.gameObject.tag == "Floor")
			{
				gameObject.transform.position = hit.point;
				m_canPlace = true;
				break;
			}
		}

		float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

		if (rotate < 0)
			gameObject.transform.Rotate(new Vector3(0, 15, 0));
		if (rotate > 0)
			gameObject.transform.Rotate(new Vector3(0, -15, 0));
	}
}
