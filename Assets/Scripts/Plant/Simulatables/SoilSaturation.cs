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

	[SerializeField] [ReadOnly] float m_saturation = 0;

	static float s_standardDrainingTime = 6; //hours

	public event SaturationEvent SaturationUpdated;

	public void Water(float amount)
	{
		m_saturation = Mathf.Clamp(m_saturation + amount, 0, 1);
	}

	public override void Initialise(InitParamsBase initParams)
	{
		m_saturation = ((InitParamsSoilSaturation)initParams).m_saturation;
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
		float dryRate = s_standardDrainingTime * (1/GetComponentInParent<Parameters.ParametersComponent>().DrainingFactor);

		float timeInHours = delta / 3600;
		float equivTime = -dryRate * Mathf.Log(Saturation);
		float totalTime = timeInHours + equivTime;
		m_saturation = Mathf.Exp(-totalTime / dryRate);
		SaturationUpdated.Invoke(Saturation, delta);
	}
}