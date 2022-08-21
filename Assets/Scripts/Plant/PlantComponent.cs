using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponent : MonoBehaviour
{
	Parameters.PlantParameters m_plantParameters;
	internal bool m_isInitialised = false;

	[SerializeField] string m_plantName;

	public string Name { get => m_plantName; }

	internal Parameters.PlantParameters Parameters { get => m_plantParameters; }

	//TODO temporary until loop for getting new plants is implemented
	public void TestInitialise()
	{
		m_plantParameters = new Parameters.PlantParameters();
		m_plantParameters.Initialise();
		m_isInitialised = true;
	}
}
