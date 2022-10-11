using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagerBase : MonoBehaviour
{
	protected List<PlantComponent> m_plants = new List<PlantComponent>();

	public static PlantManagerBase Get;

	public void Start()
	{
		if (Get == null)
			Get = this;
		else
			Destroy(this);
	}

	public void RegisterPlant(PlantComponent plant)
	{
		m_plants.Add(plant);
	}

	public void UnregisterPlant(PlantComponent plant)
	{
		if(m_plants.Contains(plant))
			m_plants.Remove(plant);	
	}
}
