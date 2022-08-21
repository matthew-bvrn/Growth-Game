using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Leaf : MonoBehaviour
{
	internal Leaf(Parameters.PlantParameters parameters)
	{
		m_plantParameters = parameters;
	}

	internal enum EState
	{
		Growing,
		Dying,
		Dead
	}

	protected Parameters.PlantParameters m_plantParameters;

	protected float m_age = 0;
	protected EState m_state = EState.Growing;

	internal EState State { get => m_state; }

	internal abstract void UpdateGrowth(float deltaGrowth, LeafParametersBase parameters);
}
