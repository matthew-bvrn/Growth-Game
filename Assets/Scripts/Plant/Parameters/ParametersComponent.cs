using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	public class ParametersComponent : MonoBehaviour
	{
		[Header("Moisture")]
		[SerializeField] internal float m_moistureNormalMean;
		[SerializeField] internal float m_moistureNormalStandardDeviation;

		UserParameters m_userParameters;
		SpeciesParameters m_speciesParameters;

		public float BaseGrowthFactor { get => m_userParameters.m_baseGrowthFactor; }
		public float PotFactor { get => m_userParameters.m_pot.SizeFactor; }
		public float DrainingFactor { get => m_userParameters.m_pot.DrainingFactor * m_userParameters.m_soil.DrainingFactor; }

		public void Initialise(bool useDefaultParameters)
		{
			m_userParameters = new UserParameters();
			m_speciesParameters = new SpeciesParameters();

			if (useDefaultParameters)
			{
				m_userParameters.Initialise();
				m_speciesParameters.Initialise(this);
			}
			//TODO else pass in parameters from whereever they will be stored
		}
	}
}