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

public class ModelHandlerRosette : ModelHandler
{
	public List<LeafRosette> Leaves { get; private set; } = new List<LeafRosette>();
	[SerializeField] int m_leafThreshold = 300;
	[SerializeField] LeafParametersRosette m_leafParameters = new LeafParametersRosette();
	[SerializeField] float m_newLeafRotIncrement = 80;
	[SerializeField] float m_newLeafHeightIncrement = 0.005f;

	public float NewLeafRot { get; private set; } = 0;
	public float PlantHeight { get; private set; } = 0;

	float m_newLeafGrowth = 0;

	List<LeafRosette> m_leafRemoveBuffer = new List<LeafRosette>();

	public sealed override void Age(float deltaSeconds)
	{
		foreach (LeafRosette leaf in Leaves)
			leaf.Age += deltaSeconds;
	}

	public sealed override void ChangePot()
	{
		foreach (LeafRosette leaf in Leaves)
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
		foreach (LeafRosette leaf in Leaves)
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
			float leafGrowth = growth - (Leaves.Count + 1) * m_leafThreshold;

			PlantHeight += m_newLeafHeightIncrement;
			NewLeafRot += m_newLeafRotIncrement + (Random.value - 0.5f) * 10;

			Object leafPrefab = Resources.Load("Prefabs/Plants/" + GetComponentInParent<PlantComponent>().Name + "Leaf");

			if (leafPrefab == null)
			{
				Debug.LogError("Leaf prefab is missing for plant with name " + GetComponentInParent<PlantComponent>().Name);
				return;
			}

			Vector3 leafPosition = m_origin.position + new Vector3(0, PlantHeight, 0);
			Quaternion leafRotation = Quaternion.Euler(m_leafParameters.m_initialRotation, NewLeafRot, 0);

			GameObject newLeaf = (GameObject)Instantiate(leafPrefab, leafPosition, leafRotation, transform);

			newLeaf.GetComponent<LeafRosette>().Initialise(GetComponentInParent<Parameters.ParametersComponent>(), PlantHeight * new Vector3(0, 1, 0));
			newLeaf.transform.localScale = new Vector3(1, 1, 1);

			m_newLeafGrowth -= m_leafThreshold;
			newLeaf.GetComponent<LeafRosette>().UpdateLeaf(m_newLeafGrowth, m_leafParameters);

			Leaves.Add(newLeaf.GetComponent<LeafRosette>());
		}

		//destroy old leaves
		foreach (LeafRosette leaf in m_leafRemoveBuffer)
		{
			Leaves.Remove(leaf);
			Destroy(leaf.gameObject);
		}

		m_leafRemoveBuffer.Clear();
	}
}
