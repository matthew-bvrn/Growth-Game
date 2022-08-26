using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeafParametersRosette : LeafParametersBase
{
	public float m_initialRotation = -50;
	public float m_maxRotation = -80;

	public float m_growthScaleSpeed = 0.01f;
	public float m_deathScaleSpeed = 0.03f;
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

	List<LeafRosette> m_leafRemoveBuffer = new List<LeafRosette>();

	internal sealed override void Simulate(float deltaSeconds)
	{
		if (!CheckInitialistion())
			return;

		GrowthComponent growthComponent = GetComponentInParent<GrowthComponent>();
		float growth = growthComponent.Growth;
		float deltaGrowth = growthComponent.DeltaGrowth;

		//update leaves
		foreach (LeafRosette leaf in m_leaves)
		{
			leaf.UpdateGrowth(deltaGrowth, m_leafParameters);
			if (leaf.State == Leaf.EState.Dead)
			{
				m_leafRemoveBuffer.Add(leaf);
			}
		}

		foreach(LeafRosette leaf in m_leafRemoveBuffer)
		{
			m_leaves.Remove(leaf);
			Destroy(leaf);
		}

		//add new leaves
		int newLeafCount = (int)growthComponent.Growth / m_leafThreshold;
		while (m_leaves.Count < newLeafCount)
		{
			float leafGrowth = growthComponent.Growth - (m_leaves.Count + 1) * m_leafThreshold;

			m_plantHeight += m_newLeafHeightIncrement;
			m_newLeafRot += m_newLeafRotIncrement;

			Object leafPrefab = Resources.Load("Prefabs/Plants/" + GetComponentInParent<PlantComponent>().Name + "Leaf");

			if (leafPrefab == null)
			{
				Debug.LogError("Leaf prefab is missing for plant with name " + GetComponentInParent<PlantComponent>().Name);
				return;
			}

			Vector3 leafPosition = transform.position + new Vector3(0, m_plantHeight, 0);
			Quaternion leafRotation = Quaternion.Euler(m_leafParameters.m_initialRotation, m_newLeafRot, 0);

			GameObject newLeaf = (GameObject)Instantiate(leafPrefab, leafPosition, leafRotation, transform);

			newLeaf.GetComponent<Leaf>().Initialise(GetComponentInParent<Parameters.ParametersComponent>());
			newLeaf.transform.localScale = new Vector3(1, 1, 1);
			newLeaf.GetComponent<Leaf>().UpdateGrowth(leafGrowth, m_leafParameters);

			m_leaves.Add(newLeaf.GetComponent<LeafRosette>());
		}
	}
}
