using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LeafRosette : Leaf
{
	internal LeafRosette(Parameters.PlantParameters parameters) : base(parameters) { }

	internal override void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		float potFactor = m_plantParameters.PotFactor;
		Vector3 onesVec = new Vector3(1, 1, 1);

		m_age += deltaGrowth;

		if (m_state == EState.Growing)
		{
			gameObject.transform.localScale += rosetteParams.m_growthScaleSpeed * m_age * onesVec;
			gameObject.transform.Rotate(deltaGrowth * rosetteParams.m_rotationSpeed * (rosetteParams.m_maxRotation - rosetteParams.m_initialRotation) / potFactor, 0, 0);

			if (m_age >= potFactor * 100)
			{
				m_state = EState.Dying;
			}

		}
		else if (m_state == EState.Dying)
		{
			gameObject.transform.localScale -= deltaGrowth * rosetteParams.m_deathScaleSpeed * onesVec;
			if (gameObject.transform.localScale.x < 0)
			{
				m_state = EState.Dead;
			}
		}
	}
}
