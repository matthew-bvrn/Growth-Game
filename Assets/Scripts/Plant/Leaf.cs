using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Leaf : MonoBehaviour
{
	public void Initialise(Parameters.PlantParameters parameters)
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
	protected float m_maxAge = 0;
	protected EState m_state = EState.Growing;

	internal EState State { get => m_state; }

	internal abstract void UpdateGrowth(float deltaGrowth, LeafParametersBase parameters);
}
