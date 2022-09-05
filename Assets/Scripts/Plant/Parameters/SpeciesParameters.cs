using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class SpeciesParameters
	{
		float m_uptakeFocus;
		float m_uptakeRange;

		public float MaxAge { get; set; }


		static float s_sqrtPi = Mathf.Sqrt(Mathf.PI);

		internal float GetWaterHealth(float value)
		{
			float output = Mathf.Max(0, 2.2f / (-Mathf.Exp(Mathf.Pow((value - m_uptakeFocus) / m_uptakeRange, 2))) + 1);
			if (value < m_uptakeFocus)
				output = -output;
			return output;
		}

		internal float GetWaterFactor(float value)
		{
			return Mathf.Clamp(BumpFunction(value, m_uptakeFocus, m_uptakeRange), 0, 1);
		}

		static float BumpFunction(float value, float focus, float range)
		{
			return 1.1f / (Mathf.Exp(Mathf.Pow((value - focus) / 2*range, 2)));
		}

		internal void Initialise(ParametersComponent component)
		{
			m_uptakeFocus = component.m_uptakeFocus;
			m_uptakeRange = component.m_uptakeRange;
			MaxAge = component.m_maxAge;
		}
	}
}
