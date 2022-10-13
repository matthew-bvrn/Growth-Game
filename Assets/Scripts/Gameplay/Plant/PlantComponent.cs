using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponent : MonoBehaviour, AdditionalDataFetcher
{

	internal bool m_isInitialised = false;

	[SerializeField] string m_plantName;

	public string Name { get => m_plantName; }

	//TODO temporary until loop for getting new plants is implemented
	public void Start()
	{
		Initialise();
		PlantManagerBase.Get.RegisterPlant(this);
	}

	public void OnDestroy()
	{
		PlantManagerBase.Get.UnregisterPlant(this);
	}

	public void Initialise()
	{
		GetComponent<Parameters.ParametersComponent>().Initialise(true);

		m_isInitialised = true;
	}

	public AdditionalData GetData()
	{
		PlantData data = new PlantData();

		data.Species = Name;
		GetComponent<GrowthComponent>().GetData(data);

		data.modelData = GetComponentInChildren<ModelHandler>().GetData();
		data.soilData = GetComponentInChildren<SoilSaturation>().GetData();
		data.waterData = GetComponentInChildren<WaterUptake>().GetData();

		return data;
	}

	public void SetData(PlantData data)
	{
		m_plantName = data.Species;
	}
}
