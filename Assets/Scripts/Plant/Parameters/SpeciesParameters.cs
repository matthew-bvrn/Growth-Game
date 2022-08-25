using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class SpeciesParameters
	{
		float m_saturationFocus;
		float m_saturationRange;

		internal float GetSaturationFactor(float value)
		{
			return Mathf.Clamp(BumpFunction(value, m_saturationFocus, m_saturationRange), 0, 1);
		}

		float BumpFunction(float value, float focus, float range)
		{
			return 1.1f / (Mathf.Exp(Mathf.Pow((value - focus) / range, 2)));
		}

		internal void Initialise(ParametersComponent component)
		{
			m_saturationFocus = component.m_saturationFocus;
			m_saturationRange = component.m_saturationRange;
		}
	}
}
