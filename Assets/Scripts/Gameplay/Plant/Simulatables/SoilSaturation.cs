using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitParamsSoilSaturation : InitParamsBase
{
	public InitParamsSoilSaturation(float saturation) => m_saturation = saturation;

	public float m_saturation = 0.5f;
}

public delegate void SaturationEvent(float saturation, float delta);

public class SoilSaturation : ISimulatable
{
	public float Saturation { get => m_saturation; }

#if UNITY_EDITOR
	[ReadOnly]
#endif
	[SerializeField] float m_saturation = 0;

	bool m_isInitialised = false;

	static float s_standardDrainingTime = 12; //hours

	public event SaturationEvent SaturationUpdated;

	public void Water(float amount)
	{
		m_saturation = Mathf.Clamp(m_saturation + amount, 0, 1);
	}

	private void Start()
	{
		SaturationUpdated += OnSaturationUpdated;
	}

	public override void Initialise(InitParamsBase initParams)
	{
		if (!m_isInitialised)
		{
			m_isInitialised = true;
			m_saturation = ((InitParamsSoilSaturation)initParams).m_saturation;
		}
		base.Initialise(initParams);
	}

	internal override void PreSimulate(float deltaSeconds)
	{
		if (!CheckInitialistion())
			return;

		UpdateSaturation(deltaSeconds);
	}

	private void UpdateSaturation(float delta)
	{
		if (GetComponentInParent<PlantComponent>().Parent == null)
		{
			float dryRate = s_standardDrainingTime * (1 / GetComponentInParent<Parameters.ParametersComponent>().DrainingFactor);

			float timeInHours = delta / 3600;
			float equivTime = -dryRate * Mathf.Log(Saturation);
			float totalTime = timeInHours + equivTime;
			m_saturation = Mathf.Exp(-totalTime / dryRate);
			SaturationUpdated.Invoke(Saturation, delta);
		}
		else
		{
			m_saturation = GetComponentInParent<PlantComponent>().Parent.GetComponentInChildren<SoilSaturation>().Saturation;
		}
	}

	internal SoilData GetData()
	{
		SoilData data = new SoilData();
		data.Saturation = Saturation;
		return data;
	}

	internal void SetData(SoilData data)
	{
		m_isInitialised = true;
		m_saturation = data.Saturation;
	}

	void OnSaturationUpdated(float saturation, float delta)
	{
		GetComponentInChildren<Parameters.Pot>().UpdateSaturation(saturation);
	}
}
