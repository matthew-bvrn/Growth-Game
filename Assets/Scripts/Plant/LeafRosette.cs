using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

internal class LeafRosette : Leaf
{
	internal override void UpdateGrowth(float deltaGrowth, LeafParametersBase leafParams)
	{
		LeafParametersRosette rosetteParams = (LeafParametersRosette)leafParams;
		m_maxAge = m_plantParameters.PotFactor * 50;
		Vector3 onesVec = new Vector3(1, 1, 1);

		m_age += deltaGrowth;

		float ageProgress = m_age / m_maxAge;

		if (m_state == EState.Growing)
		{
			gameObject.transform.localScale = rosetteParams.m_growthScaleSpeed * m_age * onesVec;
			float rotation = ageProgress * rosetteParams.m_maxRotation + (1 - ageProgress) * rosetteParams.m_initialRotation;
			gameObject.transform.rotation = Quaternion.Euler(rotation, gameObject.transform.rotation.eulerAngles.y, 0);

			float animation = ageProgress * 1 + (1 - ageProgress) * 0;
			var alembicPlayer = transform.GetChild(0).GetComponent("AlembicStreamPlayer");

			if(alembicPlayer != null)
			{
				var so = new SerializedObject(alembicPlayer);
				var prop = so.FindProperty("currentTime");
				prop.floatValue = animation;
				so.ApplyModifiedProperties();
			}

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
