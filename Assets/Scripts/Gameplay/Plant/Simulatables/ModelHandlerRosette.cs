using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeafParametersRosette : LeafParametersBase
{
	public float m_initialRotation = -50;
	public float m_maxRotation = -80;

	public float m_growthScaleSpeed = 0.03f;
	public float m_deathScaleSpeed = 0.01f;
}

public class ModelRosetteData : ModelData
{
	public List<LeafData> LeafData = new List<LeafData>();
	public float NewLeafRot;
	public float PlantHeight;
}

public class ModelHandlerRosette : ModelHandler
{
	List<LeafRosette> m_leaves = new List<LeafRosette>();
	[SerializeField] int m_leafThreshold = 300;
	[SerializeField] LeafParametersRosette m_leafParameters = new LeafParametersRosette();
	[SerializeField] float m_newLeafRotIncrement = 80;
	[SerializeField] float m_newLeafHeightIncrement = 0.005f;

	float m_newLeafRot = 0;
	float m_plantHeight = 0;

	float m_newLeafGrowth = 0;

	List<LeafRosette> m_leafRemoveBuffer = new List<LeafRosette>();

	public sealed override ModelData GetData()
	{
		ModelRosetteData modelRosetteData = new ModelRosetteData();
		modelRosetteData.NewLeafRot = m_newLeafRot;
		modelRosetteData.PlantHeight = m_plantHeight;

		foreach (Leaf leaf in m_leaves)
		{
			LeafData leafData = leaf.GetData();

			modelRosetteData.LeafData.Add(leafData);
		}

		return modelRosetteData;
	}

	public sealed override void SetData(ModelData data)
	{
		ModelRosetteData rosetteData = (ModelRosetteData)data;

		m_newLeafRot = rosetteData.NewLeafRot;
		m_plantHeight = rosetteData.PlantHeight;

		Object leafPrefab = Resources.Load("Prefabs/Plants/" + GetComponentInParent<PlantComponent>().Name + "Leaf");
		foreach (LeafData leafData in rosetteData.LeafData)
		{
			Leaf newLeaf = ((GameObject)Instantiate(leafPrefab, leafData.Position, leafData.Rotation, transform)).GetComponent<Leaf>();
			newLeaf.Initialise(GetComponentInParent<Parameters.ParametersComponent>());
			newLeaf.SetData(leafData);
			newLeaf.UpdateLeaf(0, m_leafParameters);
			m_leaves.Add((LeafRosette)newLeaf);
		}
	}

	public sealed override void Age(float deltaSeconds)
	{
		foreach (LeafRosette leaf in m_leaves)
			leaf.Age += deltaSeconds;
	}

	public sealed override void ChangePot()
	{
		foreach (LeafRosette leaf in m_leaves)
		{
			leaf.transform.position = m_origin.transform.position;
			leaf.transform.position += leaf.Offset;
		}
	}

	internal sealed override void Simulate(float growth, float deltaGrowth)
	{
		if (!CheckInitialistion())
			return;

		//update existing leaves
		foreach (LeafRosette leaf in m_leaves)
		{
			leaf.UpdateLeaf(deltaGrowth, m_leafParameters);
			if (leaf.State == ELeafState.Dead)
			{
				m_leafRemoveBuffer.Add(leaf);
			}
		}

		m_newLeafGrowth += deltaGrowth;


		while (m_newLeafGrowth > m_leafThreshold)
		{
			float leafGrowth = growth - (m_leaves.Count + 1) * m_leafThreshold;

			m_plantHeight += m_newLeafHeightIncrement;
			m_newLeafRot += m_newLeafRotIncrement + (Random.value - 0.5f) * 10;

			Object leafPrefab = Resources.Load("Prefabs/Plants/" + GetComponentInParent<PlantComponent>().Name + "Leaf");

			if (leafPrefab == null)
			{
				Debug.LogError("Leaf prefab is missing for plant with name " + GetComponentInParent<PlantComponent>().Name);
				return;
			}

			Vector3 leafPosition = m_origin.position + new Vector3(0, m_plantHeight, 0);
			Quaternion leafRotation = Quaternion.Euler(m_leafParameters.m_initialRotation, m_newLeafRot, 0);

			GameObject newLeaf = (GameObject)Instantiate(leafPrefab, leafPosition, leafRotation, transform);

			newLeaf.GetComponent<LeafRosette>().Initialise(GetComponentInParent<Parameters.ParametersComponent>(), m_plantHeight * new Vector3(0, 1, 0));
			newLeaf.transform.localScale = new Vector3(1, 1, 1);

			m_newLeafGrowth -= m_leafThreshold;
			newLeaf.GetComponent<LeafRosette>().UpdateLeaf(m_newLeafGrowth, m_leafParameters);

			m_leaves.Add(newLeaf.GetComponent<LeafRosette>());
		}

		//destroy old leaves
		foreach (LeafRosette leaf in m_leafRemoveBuffer)
		{
			m_leaves.Remove(leaf);
			Destroy(leaf.gameObject);
		}

		m_leafRemoveBuffer.Clear();
	}
}
