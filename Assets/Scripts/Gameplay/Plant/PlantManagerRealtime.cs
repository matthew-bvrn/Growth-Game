using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlantManagerRealtime : PlantManagerBase
{
	//TODO move somewhere sensible like game manager
	public static float m_testDeltaMultiplier = 500;

#if DEBUG
	public PlantComponent m_testPlant;
#endif

	public void Start()
	{
#if DEBUG
		RegisterPlant(m_testPlant);

		GameConsole.Instance.AddCommand("simulateall", DebugSimulate);
		GameConsole.Instance.AddCommand("simulatenearest", DebugSimulateNearest);
		GameConsole.Instance.AddCommand("waterall", DebugWaterAll);
		GameConsole.Instance.AddCommand("waternearest", DebugWaterNearest);
		GameConsole.Instance.AddCommand("changepotnearest", DebugChangePotNearest);
		GameConsole.Instance.AddCommand("createplant", DebugCreatePlant);
#endif
	}

	//TODO temporary, this should eventually be put into a game state manager
	public void Update()
	{
		Simulate(Time.deltaTime);
	}

	void Simulate(float delta)
	{
		delta *= m_testDeltaMultiplier;

		foreach (PlantComponent plant in m_plants)
		{
			plant.GetComponent<GrowthComponent>().Simulate(delta);
		}
	}
}
