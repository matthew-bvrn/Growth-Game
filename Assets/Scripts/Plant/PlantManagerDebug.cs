using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlantManagerRealtime : PlantManagerBase
{
	[SerializeField] FreeCam m_freeCam;

	void DebugWaterAll(int count, params string[] args)
	{
		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
		{
			plant.GetComponent<GrowthComponent>().Water();
		}
	}

	void DebugSimulateNearest(int count, params string[] args)
	{
		float amount = DebugParseSimulate(count, args);
		float delta = amount * m_testDeltaMultiplier;
		GetClosestPlant().GetComponent<GrowthComponent>().SimulatePeriod(delta);
	}

	void DebugWaterNearest(int count, params string[] args)
	{
		GetClosestPlant().GetComponent<GrowthComponent>().Water();
	}

	PlantComponent GetClosestPlant()
	{
		float minDistance = float.PositiveInfinity;
		PlantComponent minPlant = null;
		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
		{
			float distance = (plant.transform.position - m_freeCam.transform.position).magnitude;

			if (distance < minDistance)
			{
				minDistance = distance;
				minPlant = plant;
			}
		}
		return minPlant;
	}

	void DebugSimulate(int count, params string[] args)
	{
		float amount = DebugParseSimulate(count, args);
		if (amount == -1)
			return;

		float delta = amount * m_testDeltaMultiplier;
		foreach (PlantComponent plant in GetComponentsInChildren<PlantComponent>())
		{
			plant.GetComponent<GrowthComponent>().SimulatePeriod(delta);
		}
	}

	float DebugParseSimulate(int count, params string[] args)
	{
		if (count > 1)
		{
			Debug.LogError("simulateall doesn't accept this many params.");
			return -1;
		}
		else if (count == 1)
			return float.Parse(args[0]);
		else
		{
			Debug.Log("No value given, defaulting to 100 seconds");
			return 100;
		}
	}
}
