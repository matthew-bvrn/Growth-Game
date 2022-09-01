using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LeafRosette : Leaf
{
	float m_potFactor;

	public override void Initialise(Parameters.ParametersComponent parameters)
	{
		//set once so scale is only determined by pot factor when this leaf is created
		m_potFactor = parameters.PotFactor;
		m_maxAge = parameters.MaxAge;
		base.Initialise(parameters);
	}

	internal override void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		Vector3 onesVec = new Vector3(1, 1, 1);

		float ageProgress = m_age / m_maxAge;

		if (m_state == EState.Growing)
		{
			m_growth += rosetteParams.m_growthScaleSpeed * m_potFactor * deltaGrowth;
			gameObject.transform.localScale = m_growth * onesVec;
			float rotation = ageProgress * rosetteParams.m_maxRotation + (1 - ageProgress) * rosetteParams.m_initialRotation;
			gameObject.transform.rotation = Quaternion.Euler(rotation, gameObject.transform.rotation.eulerAngles.y, 0);

			GetComponent<LeafRosetteAnimationComponent>().Age = ageProgress;

			if (m_age >= m_maxAge)
			{
				m_state = EState.Dying;
			}

		}
		if (m_state == EState.Dying)
		{
			gameObject.transform.localScale -= m_deltaAge * rosetteParams.m_deathScaleSpeed * onesVec;
			if (gameObject.transform.localScale.x < 0)
			{
				m_state = EState.Dead;
			}
		}
	}
}
