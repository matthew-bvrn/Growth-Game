using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class SpeciesParameters
	{
		float m_saturationFocus;
		float m_saturationRange;

		static float s_sqrtPi = Mathf.Sqrt(Mathf.PI);

		internal float GetSaturationFactor(float value)
		{
			return Mathf.Clamp(BumpFunction(value, m_saturationFocus, m_saturationRange), 0, 1);
		}

		internal float GetAverageSaturationFactor(float valueStart, float valueEnd)
		{
			return BumpFunctionIntegral(valueEnd) - BumpFunctionIntegral(valueStart);
		}

		static float BumpFunction(float value, float focus, float range)
		{
			return 1.1f / (Mathf.Exp(Mathf.Pow((value - focus) / range, 2)));
		}

		static float Erf(float x)
		{
			// constants
			float a1 = 0.254829592f;
			float a2 = -0.284496736f;
			float a3 = 1.421413741f;
			float a4 = -1.453152027f;
			float a5 = 1.061405429f;
			float p = 0.3275911f;

			// Save the sign of x
			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Mathf.Abs(x);

			float t = 1.0f / (1.0f + p * x);
			float y = 1.0f - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-x * x);

			return sign * y;
		}

		static float BumpFunctionIntegral(float value)
		{
			return s_sqrtPi * Erf(value) / 2;
		}

		internal void Initialise(ParametersComponent component)
		{
			m_saturationFocus = component.m_saturationFocus;
			m_saturationRange = component.m_saturationRange;
		}
	}
}
