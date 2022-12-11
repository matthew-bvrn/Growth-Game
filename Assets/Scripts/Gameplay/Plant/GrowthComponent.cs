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
	static float s_sicknessMultiplier = 0.0005f;
	static int s_longTermSimulationTimestep = 1;

	public float Growth { get => m_growth; }
	public float DeltaGrowth { get => m_deltaGrowth; }
#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField] float m_sickness = 0;
	bool m_isDead = false;

	public void Start()
	{
		//TODO temp testing code, should be called when plant is created by other means
		InitParamsSoilSaturation initParams = new InitParamsSoilSaturation(0.5f);
		GetComponentInChildren<SoilSaturation>().Initialise(initParams);

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			if (!component.IsInitialised)
				component.Initialise(new InitParamsBase());

		ModelHandler modelHandler = GetComponentInChildren<ModelHandler>();
		if (!modelHandler)
			Debug.LogError("Model handler component is missing.");
	}

	public void GetData(PlantData data)
	{
		data.Age = m_age;
		data.Growth = Growth;
		data.GrowthFactor = m_growthFactor;
		data.Sickness = m_sickness;
		data.IsDead = m_isDead;
	}

	internal void SetData(PlantData data)
	{
		m_age = data.Age;
		m_growth = data.Growth;
		m_growthFactor = data.GrowthFactor;
		m_sickness = data.Sickness;
		m_isDead = data.IsDead;
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
		if (m_isDead)
			return;

		if (GetComponent<SelectableBase>().State != ESelectableState.Placed)
			return;

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.PreSimulate(deltaTime);

		CalculateGrowthFactor(deltaTime);

		m_deltaGrowth = deltaTime * m_growthFactor * s_growthMultiplier;
		m_growth += m_deltaGrowth;
		m_age += deltaTime * s_growthMultiplier;

		foreach (ISimulatable component in GetComponentsInChildren<ISimulatable>())
			component.Simulate(m_growth, m_deltaGrowth);

		foreach (IAgeable component in GetComponentsInChildren<IAgeable>())
			component.Age(deltaTime * s_growthMultiplier);
	}

	public void CalculateGrowthFactor(float deltaTime)
	{
		Parameters.ParametersComponent parametersComponent = GetComponent<Parameters.ParametersComponent>();

		float baseFactor = parametersComponent.BaseGrowthFactor;
		float growthFactor = 1;

		foreach (IGrowthAffector component in GetComponentsInChildren<IGrowthAffector>())
			growthFactor *= component.GetGrowthFactor();


		if (growthFactor < 0.5)
			m_sickness += deltaTime * 2*(0.5f-growthFactor) * s_sicknessMultiplier * s_growthMultiplier;
		else
			m_sickness -= deltaTime * 2*(growthFactor-0.5f) * s_sicknessMultiplier * s_growthMultiplier;

		m_sickness = Mathf.Clamp(m_sickness, 0, 1);

		if (m_sickness == 1)
		{
			m_isDead = true;
		}

		growthFactor *= (1 - m_sickness);

		parametersComponent.GrowthFactor = growthFactor;

		m_growthFactor = baseFactor * growthFactor;
	}
}
