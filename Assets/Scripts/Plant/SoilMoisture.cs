using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilMoisture : SimulatableBase
{
	float m_waterSaturation;

	public override void Simulate(float deltaTime)
	{
		float dryRate = GetComponentInParent<PlantComponent>().Parameters.GetDrainingFactor();

		float timeInHours = deltaTime / 3600;
		float equivTime = -dryRate * Mathf.Log(m_waterSaturation);
		float totalTime = timeInHours + equivTime;
		m_waterSaturation = Mathf.Exp(-totalTime / dryRate);
	}
}
