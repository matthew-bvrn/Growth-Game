using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagerBase : MonoBehaviour
{
	protected List<PlantComponent> m_plants = new List<PlantComponent>();

	protected void RegisterPlant(PlantComponent plant)
	{
		m_plants.Add(plant);
	}
}
