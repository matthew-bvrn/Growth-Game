using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public abstract class SelectableFreestanding : SelectableBase
{
	bool m_wasRotated = false;


	protected override void OnStateChangedInternal()
	{
		m_workingPosition = new Vector3();
	}

	protected override void UpdateObject(RaycastHit[] hits)
	{
		m_wasRotated = false;

		float rotate = InputManager.Get.GetAxis(EActions.RotateObject);

		if (rotate < 0)
		{
			gameObject.transform.Rotate(new Vector3(0, 15, 0));
			m_wasRotated = true;
			m_workingPosition = new Vector3();
		}
		if (rotate > 0)
		{
			gameObject.transform.Rotate(new Vector3(0, -15, 0));
			m_wasRotated = true;
			m_workingPosition = new Vector3();
		}

		FindPlacePoint(hits, m_wasRotated, new List<Vector3> { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) }, delegate (Vector3 pos) { gameObject.transform.position = pos; });
	}
}
