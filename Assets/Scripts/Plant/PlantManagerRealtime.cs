using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagerRealtime : PlantManagerBase
{
	double m_lastSimulationTime;

	//TODO move somewhere sensible like game manager
	public static float m_testDeltaMultiplier = 2000;

	public void Start()
	{
		m_lastSimulationTime = GetTime();
		GameConsole.Instance.AddCommand("simulateall", DebugForceSimulate);
	}

	void DebugForceSimulate(int count, params string[] args)
	{
		float amount;

		if (count > 1)
		{
			Debug.LogError("simulateall doesn't accept this many params.");
			return;
		}
		else if (count == 1)
			amount = float.Parse(args[0]);
		else
		{
			Debug.Log("No value given, defaulting to 100 seconds");
			amount = 100;
		}

		Debug.Log("Simulating for " + amount + " seconds");

		float delta = amount * m_testDeltaMultiplier;
		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
		{
			plant.GetComponent<GrowthComponent>().SimulatePeriod(delta);
		}
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
