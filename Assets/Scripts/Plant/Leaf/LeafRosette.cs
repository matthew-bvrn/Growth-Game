using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class LeafRosette : Leaf
{
	float m_potFactor;
	internal Vector3 Offset { get; private set; }

	public void Initialise(Parameters.ParametersComponent parameters, Vector3 offset)
	{
		//set once so scale is only determined by pot factor when this leaf is created
		m_potFactor = parameters.PotFactor;
		m_maxAge = parameters.MaxAge;
		Offset = offset;
		base.Initialise(parameters);
	}

	internal override void UpdateLeaf(float deltaGrowth, LeafParametersBase leafParams)
	{
		UpdateGrowth(deltaGrowth, leafParams);
		UpdateSickness();
	}

	void UpdateSickness()
	{
		float waterHealth = m_parametersComponent.WaterHealth / 2 + .5f;
		float ageFactor = Mathf.Max(0, m_ageProgress - 0.5f);

		GetComponent<LeafRosetteAnimationComponent>().SicknessLevel = m_parametersComponent.GrowthFactor;
		GetComponent<LeafRosetteShaderComponent>().UpdateShader(waterHealth, ageFactor);
	}

	void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		Vector3 onesVec = new Vector3(1, 1, 1);

		m_ageProgress = m_age / m_maxAge;

		if (m_state == EState.Growing)
		{
			m_growth += rosetteParams.m_growthScaleSpeed * m_potFactor * deltaGrowth;
			gameObject.transform.localScale = m_growth * onesVec;
			float rotation = m_ageProgress * rosetteParams.m_maxRotation + (1 - m_ageProgress) * rosetteParams.m_initialRotation;
			gameObject.transform.rotation = Quaternion.Euler(rotation, gameObject.transform.rotation.eulerAngles.y, 0);

			GetComponent<LeafRosetteAnimationComponent>().Age = m_ageProgress;

			if (m_age >= m_maxAge)
			{
				m_state = EState.Dying;
			}

		}
		if (m_state == EState.Dying)
		{
			gameObject.transform.localScale -= m_deltaAge * rosetteParams.m_deathScaleSpeed * onesVec * m_potFactor;
			if (gameObject.transform.localScale.x < 0)
			{
				m_state = EState.Dead;
			}
		}
	}
}
