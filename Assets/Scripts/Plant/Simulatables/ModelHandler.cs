using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract internal class ModelHandler : ISimulatable, IAgeable
{
	[SerializeField] protected Transform m_origin;

	void Start()
	{
		if (m_origin == null)
			Debug.LogError(GetComponentInParent<PlantComponent>().Name + " doesn't have its origin set.");
	}

	internal abstract override void Simulate(float growth, float deltaGrowth);
	public abstract void Age(float deltaSeconds);
}
