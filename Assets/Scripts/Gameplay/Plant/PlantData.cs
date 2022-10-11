using UnityEngine;

public abstract class ModelData
{ }

public class PlantData : AdditionalData
{
	public string Species;
	public float Age;
	public float Growth;
	public float GrowthFactor;
	public ModelData modelData;
}

public struct LeafData
{
	public float Age;
	public float MaxAge;
	public ELeafState State;
	public Vector3 Position;
	public Quaternion Rotation;
}