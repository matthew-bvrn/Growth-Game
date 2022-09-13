using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponent : MonoBehaviour
{

	internal bool m_isInitialised = false;

	[SerializeField] string m_plantName;

	public string Name { get => m_plantName; }

	//TODO temporary until loop for getting new plants is implemented
	public void Start()
	{
		Initialise();
	}

	public void Initialise()
	{
		GetComponent<Parameters.ParametersComponent>().Initialise(true);

		m_isInitialised = true;
	}
}
