using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parameters
{
	internal class UserParameters
	{
		internal void Initialise(Pot pot)
		{
			m_pot = pot;
			m_pot.Initialise(EPotSize.Small, EPotMaterial.Plastic); //TODO pass these in from outside

			m_soil = new Soil();
		}

		internal Pot m_pot;
		internal Soil m_soil;

		internal float m_baseGrowthFactor = 1; //TODO set this per plant
	}
}