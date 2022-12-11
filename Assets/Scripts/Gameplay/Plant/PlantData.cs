using UnityEngine;

public abstract class ModelData
{
}

public class SoilData
{
	public float Saturation;
}

public class WaterData
{
	public float Level;
	public float Sickness;
}

public class PlantData : AdditionalData
{
	public string Species;
	public float Age;
	public float Growth;
	public float GrowthFactor;
	public float Sickness;
	public bool IsDead;
	public ModelData modelData;
	public SoilData soilData;
	public WaterData waterData;

	public override void LoadData(GameObject gameObject)
	{
		gameObject.GetComponent<PlantComponent>().SetData(this);
		GrowthComponent growthComponent = gameObject.GetComponent<GrowthComponent>();
		growthComponent.SetData(this);
		gameObject.GetComponentInChildren<ModelHandler>().SetData(modelData);
		gameObject.GetComponentInChildren<SoilSaturation>().SetData(soilData);
		gameObject.GetComponentInChildren<WaterUptake>().SetData(waterData);
	}
}

public struct LeafData
{
	public float Age;
	public float MaxAge;
	public float Growth;
	public float PotFactor;
	public ELeafState State;
	public Vector3 Position;
	public Quaternion Rotation;
	public Vector3 MaxSize;
}