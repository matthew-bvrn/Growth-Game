using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELeafState
{
	Growing,
	Dying,
	Dead
}

public abstract class Leaf : MonoBehaviour
{
	virtual public void Initialise(Parameters.ParametersComponent parameters)
	{
		m_parametersComponent = parameters;
	}

	protected Parameters.ParametersComponent m_parametersComponent;

	protected float m_growth = 0;
	public float Age 
	{ get => m_age; 
		set 
		{
			m_deltaAge = value - m_age;
			m_age = value;
		} 
	}

	protected float m_age = 0;
	protected float m_deltaAge = 0;
	public float MaxAge { get; protected set; } = 0;
	protected float m_ageProgress = 0;
	protected ELeafState m_state = ELeafState.Growing;

	public ELeafState State { get => m_state; }

	internal abstract void UpdateLeaf(float deltaGrowth, LeafParametersBase parameters);
}
