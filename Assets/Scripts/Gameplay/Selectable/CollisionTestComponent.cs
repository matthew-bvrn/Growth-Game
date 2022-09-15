using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerExited(Collider other);

public class CollisionTestComponent : MonoBehaviour
{
	public TriggerExited m_callback;

	public void OnTriggerExit(Collider other)
	{
		m_callback.Invoke(other);
	}
}
