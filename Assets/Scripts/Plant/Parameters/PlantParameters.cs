using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	public class PlantParameters
	{
		public void Initialise()
		{
			m_pot = new Pot(EPotSize.Small, EPotMaterial.Plastic);
			m_soil = new Soil();
		}

		Pot m_pot;
		Soil m_soil;

		float m_baseGrowthFactor = 1; //TODO set this per plant

		public float BaseGrowthFactor { get => m_baseGrowthFactor; }
		public float PotFactor { get => m_pot.SizeFactor; }
		public float GetDrainingFactor()
		{
			return m_pot.DrainingFactor * m_soil.DrainingFactor;
		}
	}
}