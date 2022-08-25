using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent : MonoBehaviour
{
	[SerializeField] float m_growth = 0;
	float m_deltaGrowth = 0;
	[ReadOnly] [SerializeField] float m_growthFactor;

	public float Growth { get => m_growth; }
	public float DeltaGrowth { get => m_deltaGrowth; }

	public void Start()
	{
		ModelHandler modelHandler = GetComponentInChildren<ModelHandler>();
		if (!modelHandler)
			Debug.LogError("Model handler component is missing.");
	}

	public void Simulate(float deltaSeconds)
	{
		if (!GetComponent<PlantComponent>().m_isInitialised)
		{
			Debug.LogWarning("Plant component not initialised, simulation wont occur");
			return;
		}

		CalculateGrowthFactor();
		m_deltaGrowth = deltaSeconds * m_growthFactor;
		m_growth += m_deltaGrowth;

		foreach(SimulatableBase parameter in GetComponentsInChildren<SimulatableBase>())
		{
			parameter.Simulate(deltaSeconds);
		}
	}

	public void CalculateGrowthFactor()
	{
		Parameters.ParametersComponent parametersComponent = GetComponent<Parameters.ParametersComponent>();

		float baseFactor = parametersComponent.BaseGrowthFactor;
		float saturation = GetComponentInChildren<SoilSaturation>().SaturationRollingAverage;

		float saturationFactor = parametersComponent.GetSaturationFactor(saturation);

		m_growthFactor = baseFactor * saturationFactor;
	}
}
