using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LeafRosette : Leaf
{
	internal override void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		m_maxAge = m_plantParameters.PotFactor * 50;
		Vector3 onesVec = new Vector3(1, 1, 1);

		m_age += deltaGrowth;

		if (m_state == EState.Growing)
		{
			gameObject.transform.localScale = rosetteParams.m_growthScaleSpeed * m_age * onesVec;
			gameObject.transform.rotation = Quaternion.Euler(m_age/m_maxAge * rosetteParams.m_maxRotation + (1-m_age/m_maxAge)*rosetteParams.m_initialRotation, gameObject.transform.rotation.eulerAngles.y, 0);

			if (m_age >= m_maxAge)
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
