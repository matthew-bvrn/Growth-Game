using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class UserParameters
	{
		internal void Initialise()
		{
			m_pot = new Pot(EPotSize.Small, EPotMaterial.Plastic);
			m_soil = new Soil();
		}

		internal Pot m_pot;
		internal Soil m_soil;

		internal float m_baseGrowthFactor = 1; //TODO set this per plant
	}
}