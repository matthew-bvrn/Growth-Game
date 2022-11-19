using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUptake : MonoBehaviour, IGrowthAffector
{
	public float WaterLevel { get => m_waterLevel; }
	bool m_isInitialised = false;

#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField] float m_waterLevel;

#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField] float m_waterSickness;

	static float s_waterUptakeMultiplier = 0.02f / PlantManagerRealtime.m_testDeltaMultiplier;

	public float GetGrowthFactor()
	{
		return 1 - Mathf.Abs(m_waterSickness);
	}

	void Start()
	{
		GetComponent<SoilSaturation>().SaturationUpdated += UpdateWaterUptake;

		if (!m_isInitialised)
		{
			m_isInitialised = true;
			m_waterLevel = 0.5f;
		}
	}

	void UpdateWaterUptake(float saturation, float deltaSeconds)
	{
		float uptakeRate = GetComponentInParent<Parameters.ParametersComponent>().UptakeRate;
		float diff = saturation - WaterLevel;

		m_waterLevel += s_waterUptakeMultiplier * deltaSeconds * diff * uptakeRate;
		m_waterSickness = GetComponentInParent<Parameters.ParametersComponent>().UpdateWaterHealth(m_waterLevel);
		GetComponentInParent<Parameters.ParametersComponent>().WaterLevel = m_waterLevel;
	}

	internal WaterData GetData()
	{
		WaterData data = new WaterData();
		data.Level = m_waterLevel;
		data.Sickness = m_waterSickness;
		return data;
	}

	internal void SetData(WaterData data)
	{
		m_waterLevel = data.Level;
		m_waterSickness = data.Sickness;
		m_isInitialised = true;
	}
}
