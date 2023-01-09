using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRosetteShaderComponent : MonoBehaviour
{
	[SerializeField] MeshRenderer m_mesh;

	internal void UpdateShader(float waterHealth, float ageFactor, bool isChild)
	{
		m_mesh.material.SetFloat("WaterHealth", waterHealth);
		if(!isChild)
			m_mesh.material.SetFloat("AgeFactor", Mathf.Min(ageFactor, 1));
	}
}
