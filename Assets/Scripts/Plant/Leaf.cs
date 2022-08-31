using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Leaf : MonoBehaviour
{
	virtual public void Initialise(Parameters.ParametersComponent parameters)
	{
		m_parametersComponent = parameters;
	}

	internal enum EState
	{
		Growing,
		Dying,
		Dead
	}

	protected Parameters.ParametersComponent m_parametersComponent;

	protected float m_age = 0;
	protected float m_maxAge = 0;
	protected EState m_state = EState.Growing;

	internal EState State { get => m_state; }

	internal abstract void UpdateGrowth(float deltaGrowth, LeafParametersBase parameters);
}
