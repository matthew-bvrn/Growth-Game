using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent : MonoBehaviour
{
	float m_growth;

	public void Start()
	{
		ModelHandler modelHandler = GetComponentInChildren<ModelHandler>();
		if (!modelHandler)
			Debug.LogError("Model handler component is missing.");
	}

	public void Simulate(float delta)
	{
		foreach(SimulatableBase parameter in GetComponentsInChildren<SimulatableBase>())
		{
			parameter.Simulate(delta);
		}
	}
}
