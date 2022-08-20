using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent : MonoBehaviour
{
	float m_growth;

  public void Simulate(float delta)
	{
		foreach(ParameterBase parameter in GetComponentsInChildren<ParameterBase>())
		{
			parameter.Simulate(float delta);
		}

		ModelHandler modelHandler = GetComponentInChildren<ModelHandler>();

		if (modelHandler)
			modelHandler.UpdateState();
		else
			Debug.LogError("Model handler component is missing.");
	}
}
