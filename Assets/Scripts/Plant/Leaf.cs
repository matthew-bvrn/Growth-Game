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

	protected float m_growth = 0;
	internal float Age 
	{ get => m_age; 
		set 
		{
			m_deltaAge = value - m_age;
			m_age = value;
		} 
	}

	protected float m_age = 0;
	protected float m_deltaAge = 0;
	protected float m_maxAge = 0;
	protected EState m_state = EState.Growing;

	internal EState State { get => m_state; }

	internal abstract void UpdateLeaf(float deltaGrowth, LeafParametersBase parameters);
}
