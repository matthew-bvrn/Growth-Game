using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUptake : MonoBehaviour
{
	public float WaterLevel { get => m_waterLevel; }
	[SerializeField] [ReadOnly] float m_waterLevel;

	public float WaterHealth { get => m_waterHealth; }
	[SerializeField] [ReadOnly] float m_waterHealth;

	static float s_waterUptakeMultiplier = 0.05f / PlantManagerRealtime.m_testDeltaMultiplier;

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
		m_waterHealth = GetComponentInParent<Parameters.ParametersComponent>().GetWaterHealth(m_waterLevel);
	}
}
