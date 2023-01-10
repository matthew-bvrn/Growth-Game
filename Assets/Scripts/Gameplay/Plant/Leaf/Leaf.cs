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

	public ELeafState State { get => m_state; }

	protected float m_growth = 0;
	internal float Age { get; set; } = 0;
	protected float m_maxAge = 0;
	public float AgeProgress { get; protected set; } = 0;
	protected float m_potFactor;
	protected Vector3 m_maxSize;
	protected ELeafState m_state = ELeafState.Growing;
	internal bool m_isSettingData = false;
	 
	internal abstract void UpdateLeaf(float deltaGrowth, LeafParametersBase parameters, bool isChild);

	internal LeafData GetData()
	{
		LeafData leafData = new LeafData();

		leafData.Age = Age;
		leafData.MaxAge = m_maxAge;
		leafData.Growth = m_growth;
		leafData.State = State;
		leafData.PotFactor = m_potFactor;
		leafData.Position = transform.localPosition;
		leafData.Rotation = transform.localRotation;
		leafData.MaxSize = m_maxSize;

		return leafData;
	}

	internal void SetData(LeafData data)
	{
		Age = data.Age;
		m_maxAge = data.MaxAge;
		m_state = data.State;
		m_growth = data.Growth;
		m_potFactor = data.PotFactor;
		m_maxSize = data.MaxSize;
	}
}
