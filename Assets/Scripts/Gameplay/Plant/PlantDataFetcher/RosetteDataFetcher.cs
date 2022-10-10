using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRosetteData : ModelData
{
	public List<LeafData> LeafData = new List<LeafData>();
	public float NewLeafRot;
	public float PlantHeight;
}

public class RosetteDataFetcher : ModelDataFetcher
{
	public override ModelData GetData()
	{
		ModelHandlerRosette component = GetComponent<ModelHandlerRosette>();

		ModelRosetteData modelRosetteData = new ModelRosetteData();
		modelRosetteData.NewLeafRot = component.NewLeafRot;
		modelRosetteData.PlantHeight = component.PlantHeight;

		foreach (Leaf leaf in component.Leaves)
		{
			LeafData leafData = new LeafData();
			leafData.Age = leaf.Age;
			leafData.MaxAge = leaf.MaxAge;
			leafData.State = leaf.State;
			leafData.Position = leaf.transform.localPosition;
			leafData.Rotation = leaf.transform.localRotation;

			modelRosetteData.LeafData.Add(leafData);
		}

		return modelRosetteData;
	}
}
