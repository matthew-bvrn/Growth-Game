using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SurfaceDestroyed();

public class SurfaceComponent : MonoBehaviour
{
	public SurfaceDestroyed OnSurfaceDestroyed;

	private void OnDestroy()
	{
		OnSurfaceDestroyed.Invoke();
	}
}
