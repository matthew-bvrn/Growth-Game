using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class SpeciesParameters
	{
		float m_moistureNormalMean;
		float m_moistureNormalStandardDeviation;

		internal void Initialise(ParametersComponent component)
		{
			m_moistureNormalMean = component.m_moistureNormalMean;
			m_moistureNormalStandardDeviation = component.m_moistureNormalStandardDeviation;
		}
	}
}
