using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBobber : MonoBehaviour
{
	[SerializeField] float m_speedSeconds;
	[SerializeField] float m_phase;
	[SerializeField] float m_displacement;

	float m_timer;
	Vector3 m_pos;

	// Start is called before the first frame update
	void Start()
	{
		m_pos = GetComponent<RectTransform>().position;
	}

	// Update is called once per frame
	void Update()
	{
		m_timer += Time.deltaTime;
		GetComponent<RectTransform>().position = m_pos + new Vector3(0, m_phase + Mathf.Sin(Mathf.PI * m_timer / m_speedSeconds), 0)*m_displacement;
	}
}
