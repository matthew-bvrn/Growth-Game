using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent : MonoBehaviour
{
	[SerializeField] float m_age = 0;
	[SerializeField] float m_growth = 0;
	float m_deltaGrowth = 0;
#if UNITY_EDITOR
	[ReadOnly]
#endif 
	[SerializeField] float m_growthFactor;

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

	public void Water()
	{
		GetComponentInChildren<SoilSaturation>().Water(0.3f);
	}

	public void SimulatePeriod(float deltaSeconds)
	{
		float deltaCopy = deltaSeconds;

		float timestep = s_longTermSimulationTimestep * PlantManagerRealtime.m_testDeltaMultiplier;

		while (deltaCopy > 0)
		{
			var value = deltaCopy > timestep ? timestep : deltaCopy;
			deltaCopy -= timestep;

			SimulateImpl(value);
		}
	}

	public void Simulate(float deltaSeconds)
	{
		if (!GetComponent<PlantComponent>().m_isInitialised)
		{
			Debug.LogWarning("Plant component not initialised, simulation wont occur");
			return;
		}

		SimulateImpl(deltaSeconds);
	}

	void SimulateImpl(float deltaTime)
	{
		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.PreSimulate(deltaTime);

		CalculateGrowthFactor(GetComponentInChildren<WaterUptake>().WaterLevel);

		m_deltaGrowth = deltaTime * m_growthFactor * s_growthMultiplier;
		m_growth += m_deltaGrowth;
		m_age += deltaTime * s_growthMultiplier;

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.Simulate(m_growth, m_deltaGrowth);

		foreach (IAgeable component in GetComponentsInChildren<IAgeable>())
			component.Age(deltaTime * s_growthMultiplier);
	}

	public void CalculateGrowthFactor(float waterLevel)
	{
		Parameters.ParametersComponent parametersComponent = GetComponent<Parameters.ParametersComponent>();

		float baseFactor = parametersComponent.BaseGrowthFactor;
		float growthFactor = 1;

		foreach (IGrowthAffector component in GetComponentsInChildren<IGrowthAffector>())
			growthFactor *= component.GetGrowthFactor();

		parametersComponent.GrowthFactor = growthFactor;

		m_growthFactor = baseFactor * growthFactor;
	}
}
