using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilMoisture : SimulatableBase
{
	public float SaturationRollingAverage { get; private set; }

	float m_saturation;

	List<float> m_saturationPollValues = new List<float>();

	float m_timeSinceLastPoll = 0;

	static float s_pollInterval = 30; //seconds
	static float s_standardDrainingTime = 6; //hours
	static int s_maxPollValues = 30;

	public override void Simulate(float deltaTime)
	{
		//loop to add poll values 
		while(deltaTime > s_pollInterval)
		{
			UpdateSaturation(s_pollInterval);
			deltaTime -= s_pollInterval;
			AddPollValue();
		}

		UpdateSaturation(deltaTime);

		m_timeSinceLastPoll += deltaTime;

		if(m_timeSinceLastPoll >= s_pollInterval)
		{
			AddPollValue();
			m_timeSinceLastPoll -= s_pollInterval;
		}
	}

	private void AddPollValue()
	{
		m_saturationPollValues.Add(m_saturation);
		if(m_saturationPollValues.Count > s_maxPollValues)
		{
			m_saturationPollValues.RemoveAt(0);
		}

		//update rolling average
		SaturationRollingAverage = 0;
		foreach (float value in m_saturationPollValues)
		{
			SaturationRollingAverage += value;
		}

		SaturationRollingAverage /= m_saturationPollValues.Count;
	}

	private void UpdateSaturation(float delta)
	{
		float dryRate = s_standardDrainingTime * (1/GetComponentInParent<PlantComponent>().Parameters.GetDrainingFactor());

		float timeInHours = delta / 3600;
		float equivTime = -dryRate * Mathf.Log(m_saturation);
		float totalTime = timeInHours + equivTime;
		m_saturation = Mathf.Exp(-totalTime / dryRate);
	}
}
