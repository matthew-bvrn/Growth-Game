using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRosetteShaderComponent : MonoBehaviour
{
	[SerializeField] MeshRenderer m_mesh;

	internal void UpdateShader(float waterHealth)
	{
		m_mesh.material.SetFloat("WaterHealth", waterHealth);
	}
}
