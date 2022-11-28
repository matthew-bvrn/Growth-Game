using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRosette : Leaf
{
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
		float ageFactor = Mathf.Max(0, AgeProgress - 0.5f);
		
		GetComponent<LeafRosetteAnimationComponent>().SicknessLevel = m_parametersComponent.GrowthFactor;
		GetComponent<LeafRosetteShaderComponent>().UpdateShader(waterHealth, ageFactor);
	}

	void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		Vector3 onesVec = new Vector3(1, 1, 1);

		AgeProgress = Age / m_maxAge;

		if (AgeProgress < 2)
		{
			float rotation = AgeProgress * rosetteParams.m_maxRotation + (1 - AgeProgress) * rosetteParams.m_initialRotation;
			gameObject.transform.rotation = Quaternion.Euler(rotation, gameObject.transform.rotation.eulerAngles.y, 0);
		}

		if (m_state == ELeafState.Growing)
		{
			m_growth += rosetteParams.m_growthScaleSpeed * m_potFactor * deltaGrowth;
			gameObject.transform.localScale = m_growth * onesVec;

			GetComponent<LeafRosetteAnimationComponent>().Age = AgeProgress;

			if (Age >= m_maxAge)
			{
				m_maxSize = gameObject.transform.localScale;
				m_state = ELeafState.Dying;
			}

		}
		if (m_state == ELeafState.Dying && gameObject.transform.localScale.x > rosetteParams.m_deadLeafSize)
		{
			gameObject.transform.localScale = m_maxSize - (Age - m_maxAge) * rosetteParams.m_deathScaleSpeed * onesVec * m_potFactor;
			if (gameObject.transform.localScale.x < 0)
			{
				m_state = ELeafState.Dead;
			}
		}
	}
}
