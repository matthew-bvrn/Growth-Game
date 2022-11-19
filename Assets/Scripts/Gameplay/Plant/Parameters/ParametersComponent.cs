using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
		[SerializeField] private Pot m_pot;

		//TODO Move into internal parameters class?
		static float s_uptakeMultiplier = 1;

		UserParameters m_userParameters;
		SpeciesParameters m_speciesParameters;

		public UnityEvent OnPotChanged;

		public EPotSize PotSize { set { m_userParameters.m_pot.Size = value; OnPotChanged.Invoke(); } }
		public float BaseGrowthFactor { get => m_userParameters.m_baseGrowthFactor; }
		public float PotFactor { get => m_userParameters.m_pot.SizeFactor; }
		public float DrainingFactor { get => m_userParameters.m_pot.DrainingFactor * m_userParameters.m_soil.DrainingFactor; }

		public float MaxAge { get => m_speciesParameters.MaxAge; }

		public float UptakeRate { get => m_baseUptakeRate * s_uptakeMultiplier; }

		public float GrowthFactor { get; set; }
		public float WaterLevel { get; set; }
		public float WaterHealth { get; private set; }

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

			if (useDefaultParameters)
			{
				m_userParameters.Initialise(m_pot);
				m_speciesParameters.Initialise(this);
			}
			//TODO else pass in parameters from whereever they will be stored
		}
	}
}