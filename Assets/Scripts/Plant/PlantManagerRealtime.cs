using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagerRealtime : PlantManagerBase
{
	double m_lastSimulationTime;
	List<PlantComponent> m_plants;

	void Simulate()
	{
		double time = GetTime();
		float delta = (float)(time - m_lastSimulationTime);

		foreach(PlantComponent plant in m_plants)
		{
			plant.GetComponent<GrowthComponent>().Simulate(delta);
		}

		m_lastSimulationTime = time;
	}

	double GetTime()
	{
		return DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}
}
