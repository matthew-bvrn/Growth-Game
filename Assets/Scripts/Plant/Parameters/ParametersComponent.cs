using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	public class ParametersComponent : MonoBehaviour
	{
		[Header("Water uptake")]
		[SerializeField] internal float m_uptakeFocus;
		[SerializeField] internal float m_uptakeRange;
		[SerializeField] internal float m_baseUptakeRate;

		[Header("Misc")]
		[SerializeField] internal float m_maxAge;

		//TODO Move into internal parameters class?
		float m_uptakeFactor;

		UserParameters m_userParameters;
		SpeciesParameters m_speciesParameters;

		public EPotSize PotSize { set { m_userParameters.m_pot.Size = value;} }
		public float BaseGrowthFactor { get => m_userParameters.m_baseGrowthFactor; }
		public float PotFactor { get => m_userParameters.m_pot.SizeFactor; }
		public float DrainingFactor { get => m_userParameters.m_pot.DrainingFactor * m_userParameters.m_soil.DrainingFactor; }
		public float SicknessFactor { get; set; }

		public float UptakeRate { get => m_baseUptakeRate * m_uptakeFactor; }

		public float WaterHealth { get; private set; }

		public float MaxAge { get => m_speciesParameters.MaxAge; }

		public float UpdateWaterHealth(float value)
		{
			WaterHealth = m_speciesParameters.GetWaterHealth(value);
			return WaterHealth;
		}

		public float GetWaterFactor(float value)
		{
			return m_speciesParameters.GetWaterFactor(value);
		}

		public void Initialise(bool useDefaultParameters)
		{
			m_userParameters = new UserParameters();
			m_speciesParameters = new SpeciesParameters();

			m_uptakeFactor = 1;

			if (useDefaultParameters)
			{
				m_userParameters.Initialise();
				m_speciesParameters.Initialise(this);
			}
			//TODO else pass in parameters from whereever they will be stored
		}
	}
}