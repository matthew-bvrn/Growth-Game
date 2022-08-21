using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeafParametersRosette : LeafParametersBase
{
	public float m_initialRotation = -90;
	public float m_maxRotation = -65;

	public float m_rotationSpeed = 0.01f;
	public float m_growthScaleSpeed = 0.01f;
	public float m_deathScaleSpeed = 0.03f;
}

public class ModelHandlerRosette : ModelHandler
{
	List<LeafRosette> m_leaves;
	[SerializeField] int m_leafThreshold = 300;
	[SerializeField] LeafParametersRosette m_leafParameters = new LeafParametersRosette();

	public sealed override void Simulate(float deltaSeconds)
	{
		GrowthComponent growthComponent = GetComponentInParent<GrowthComponent>();
		float growth = growthComponent.Growth;
		float deltaGrowth = growthComponent.DeltaGrowth;

		foreach (LeafRosette leaf in m_leaves)
		{
			leaf.UpdateGrowth(deltaGrowth, m_leafParameters);
			if (leaf.State == Leaf.EState.Dead)
			{
				m_leaves.Remove(leaf);
				Destroy(leaf);
			}
		}
	}
}
