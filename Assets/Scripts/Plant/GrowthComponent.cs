using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent : MonoBehaviour
{
	[SerializeField] float m_growth = 0;
	float m_deltaGrowth = 0;
	[ReadOnly] [SerializeField] float m_growthFactor;

	static float s_growthMultiplier = 0.05f;
	static int s_longTermSimulationTimestep = 1;

	public float Growth { get => m_growth; }
	public float DeltaGrowth { get => m_deltaGrowth; }

	public void Start()
	{
		//TODO temp testing code, should be called when plant is created by other means
		InitParamsSoilSaturation initParams = new InitParamsSoilSaturation(0.5f);
		GetComponentInChildren<SoilSaturation>().Initialise(initParams);

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			if(!component.IsInitialised)
				component.Initialise(new InitParamsBase());

		ModelHandler modelHandler = GetComponentInChildren<ModelHandler>();
		if (!modelHandler)
			Debug.LogError("Model handler component is missing.");
	}

	public void SimulatePeriod(float deltaSeconds)
	{
		float waterRollingAverage = 0;

		float deltaCopy = deltaSeconds;

		float timestep = s_longTermSimulationTimestep * PlantManagerRealtime.m_testDeltaMultiplier;
		int i = 0;

		while (deltaCopy > 0)
		{
			foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
				component.PreSimulate(deltaCopy > timestep ? timestep : deltaCopy);

			deltaCopy -= timestep;
			i++;

			CalculateGrowthFactor(GetComponentInChildren<WaterUptake>().WaterLevel);
			var value = deltaCopy > timestep ? timestep : deltaCopy;
			m_growth += value * m_growthFactor * s_growthMultiplier;
		}

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.Simulate(m_deltaGrowth, m_growth);
	}

	public void Simulate(float deltaSeconds)
	{
		if (!GetComponent<PlantComponent>().m_isInitialised)
		{
			Debug.LogWarning("Plant component not initialised, simulation wont occur");
			return;
		}

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.PreSimulate(deltaSeconds);

		CalculateGrowthFactor(GetComponentInChildren<WaterUptake>().WaterLevel);
		m_deltaGrowth = deltaSeconds * m_growthFactor * s_growthMultiplier;
		m_growth += m_deltaGrowth;

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.Simulate(m_growth, m_deltaGrowth);
	}

	public void CalculateGrowthFactor(float waterLevel)
	{
		Parameters.ParametersComponent parametersComponent = GetComponent<Parameters.ParametersComponent>();

		float baseFactor = parametersComponent.BaseGrowthFactor;
		float waterFactor = parametersComponent.GetWaterFactor(waterLevel);

		m_growthFactor = baseFactor * waterFactor;
	}
}
