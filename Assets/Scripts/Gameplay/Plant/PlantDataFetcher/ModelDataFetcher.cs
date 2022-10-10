using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LeafData
{
	public float Age;
	public float MaxAge;
	public ELeafState State;
	public Vector3 Position;
	public Quaternion Rotation;
}

public abstract class ModelDataFetcher : MonoBehaviour
{
	public abstract ModelData GetData();
}
