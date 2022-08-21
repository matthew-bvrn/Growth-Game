using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagerRealtime : PlantManagerBase
{
	double m_lastSimulationTime;

	[SerializeField] float m_testDeltaMultiplier = 100;

	public void Start()
	{
		m_lastSimulationTime = GetTime();
	}

	//TODO temporary, this should eventually be put into a game state manager
	public void Update()
	{
		Simulate();
	}

	void Simulate()
	{
		double time = GetTime();
		float delta = (float)(time - m_lastSimulationTime) * m_testDeltaMultiplier;

		foreach(PlantComponent plant in GetComponentsInChildren<PlantComponent>())
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
