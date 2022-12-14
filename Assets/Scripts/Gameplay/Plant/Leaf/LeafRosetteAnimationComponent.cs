using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LeafRosetteAnimationComponent : MonoBehaviour
{
	//range of 0-1, scaled up to animation length
	[SerializeField] float m_growthRangeStart;
	[SerializeField] float m_growthRangeEnd;

	float m_sicknessLevel;

	float m_animationStartTime;
	float m_animationEndTime;

	float m_ageValue;

	bool m_flag = false;

	public float Age
	{
		set
		{
			m_ageValue = Mathf.Clamp(value, 0, 1);
			m_flag = true;
		}
	}

	public float SicknessLevel
	{
		set
		{
			m_sicknessLevel = Mathf.Clamp(value, 0, 1);
			m_flag = true;
		}
	}

	private void Update()
	{
		if(m_flag)
		{
			UpdateAnimation();
			m_flag = false;
		}
	}

	void UpdateAnimation()
	{
		var alembicPlayer = transform.GetChild(0).GetComponent("AlembicStreamPlayer");

		float overallPos = Mathf.Clamp(m_ageValue + m_sicknessLevel, 0, 1);

		float value = (overallPos * m_growthRangeEnd) + (1 - overallPos) * m_growthRangeStart;

		if (alembicPlayer != null)
		{
			//var so = new SerializedObject(alembicPlayer); //TODO fix this
			//var prop = so.FindProperty("currentTime");
			//prop.floatValue = value;
			//so.ApplyModifiedProperties();
		}
	}
}
