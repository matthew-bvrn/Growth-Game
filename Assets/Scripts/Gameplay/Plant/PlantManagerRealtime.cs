using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlantManagerRealtime : PlantManagerBase
{
	//TODO move somewhere sensible like game manager
	public static float m_testDeltaMultiplier = 2000;

	public void Start()
	{
		GameConsole.Instance.AddCommand("simulateall", DebugSimulate);
		GameConsole.Instance.AddCommand("simulatenearest", DebugSimulateNearest);
		GameConsole.Instance.AddCommand("waterall", DebugWaterAll);
		GameConsole.Instance.AddCommand("waternearest", DebugWaterNearest);
		GameConsole.Instance.AddCommand("changepotnearest", DebugChangePotNearest);
		GameConsole.Instance.AddCommand("createplant", DebugCreatePlant);
	}

	//TODO temporary, this should eventually be put into a game state manager
	public void Update()
	{
		Simulate(Time.deltaTime);
	}

	void Simulate(float delta)
	{
		delta *= m_testDeltaMultiplier;

		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
		{
			plant.GetComponent<GrowthComponent>().Simulate(delta);
		}
	}
}
