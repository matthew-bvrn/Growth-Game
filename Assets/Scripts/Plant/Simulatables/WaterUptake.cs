using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUptake : MonoBehaviour, IGrowthAffector
{
	public float WaterLevel { get => m_waterLevel; }
	[SerializeField] [ReadOnly] float m_waterLevel;

	[SerializeField] [ReadOnly] float m_waterSickness;

	static float s_waterUptakeMultiplier = 0.02f / PlantManagerRealtime.m_testDeltaMultiplier;

	public float GetGrowthFactor()
	{
		return 1 - Mathf.Abs(m_waterSickness);
	}

	void Start()
	{
		GetComponent<SoilSaturation>().SaturationUpdated += UpdateWaterUptake;
		m_waterLevel = 0.5f;
	}

	void UpdateWaterUptake(float saturation, float deltaSeconds)
	{
		float uptakeRate = GetComponentInParent<Parameters.ParametersComponent>().UptakeRate;
		float diff = saturation - WaterLevel;

		m_waterLevel += s_waterUptakeMultiplier * deltaSeconds * diff * uptakeRate;
		m_waterSickness = GetComponentInParent<Parameters.ParametersComponent>().UpdateWaterHealth(m_waterLevel);
	}
}
