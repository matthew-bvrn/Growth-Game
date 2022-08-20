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

		public float GetDrainingFactor()
		{
			return m_pot.GetDrainingFactor() * m_soil.GetDrainingFactor();
		}
	}
}