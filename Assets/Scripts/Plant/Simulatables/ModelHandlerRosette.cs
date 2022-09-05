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

internal class ModelHandlerRosette : ModelHandler
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

	public sealed override void Age(float deltaSeconds)
	{
		foreach (LeafRosette leaf in m_leaves)
			leaf.Age += deltaSeconds;
	}

	internal sealed override void Simulate(float growth, float deltaGrowth)
	{
		if (!CheckInitialistion())
			return;

		//update existing leaves
		foreach (LeafRosette leaf in m_leaves)
		{
			leaf.UpdateLeaf(deltaGrowth, m_leafParameters);
			if (leaf.State == Leaf.EState.Dead)
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

			Vector3 leafPosition = transform.position + new Vector3(0, m_plantHeight, 0);
			Quaternion leafRotation = Quaternion.Euler(m_leafParameters.m_initialRotation, m_newLeafRot, 0);

			GameObject newLeaf = (GameObject)Instantiate(leafPrefab, leafPosition, leafRotation, transform);

			newLeaf.GetComponent<Leaf>().Initialise(GetComponentInParent<Parameters.ParametersComponent>());
			newLeaf.transform.localScale = new Vector3(1, 1, 1);

			newLeaf.GetComponent<LeafRosette>().UpdateLeaf(m_newLeafGrowth, m_leafParameters);
			m_newLeafGrowth -= m_leafThreshold;

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
