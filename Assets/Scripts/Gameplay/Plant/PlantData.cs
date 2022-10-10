using UnityEngine;

public abstract class ModelData
{ }

public struct PlantData
{
	public string Species;
	public float Age;
	public float Growth;
	public float GrowthFactor;
}

public struct LeafData
{
	public float Age;
	public float MaxAge;
	public ELeafState State;
	public Vector3 Position;
	public Quaternion Rotation;
}