using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlantManagerRealtime : PlantManagerBase
{
	double m_lastSimulationTime;

	//TODO move somewhere sensible like game manager
	public static float m_testDeltaMultiplier = 2000;

	public void Start()
	{
		m_lastSimulationTime = GetTime();
		GameConsole.Instance.AddCommand("simulateall", DebugSimulate);
		GameConsole.Instance.AddCommand("simulatenearest", DebugSimulateNearest);
		GameConsole.Instance.AddCommand("waterall", DebugWaterAll);
		GameConsole.Instance.AddCommand("waternearest", DebugWaterNearest);
		GameConsole.Instance.AddCommand("changepotnearest", DebugChangePotNearest);
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

		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
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
