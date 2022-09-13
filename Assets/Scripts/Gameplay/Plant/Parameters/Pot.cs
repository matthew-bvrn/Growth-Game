using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Parameters
{
	public enum EPotSize
	{
		Tiny,
		Small,
		Medium,
		Large,
		Giant
	}

	public enum EPotMaterial
	{
		Plastic,
		Terracotta
	}

	internal class Pot : MonoBehaviour
	{
		internal void Initialise(EPotSize _size, EPotMaterial _material)
		{
			Size = _size;
			Material = _material;
		}

		//TODO make these data driven
		static Dictionary<EPotSize, float> s_potSizeFactor = new Dictionary<EPotSize, float>()
		{
			{EPotSize.Tiny, 0.5f},
			{EPotSize.Small, 1},
			{EPotSize.Medium, 1.5f},
			{EPotSize.Large, 2.5f},
			{EPotSize.Giant, 5},
		};

		static Dictionary<EPotMaterial, float> s_potMaterialDrainingFactor = new Dictionary<EPotMaterial, float>()
		{
			{EPotMaterial.Plastic, 0.8f},
			{EPotMaterial.Terracotta, 1.2f}
		};

		internal EPotSize Size { get => m_size; set { m_size = value; transform.localScale = s_potSizeFactor[value] * new Vector3(1, 1, 1);} }
		internal EPotMaterial Material { get; set; }
		private EPotSize m_size;

		internal float SizeFactor { get => s_potSizeFactor[Size]; }
		internal float DrainingFactor { get => s_potMaterialDrainingFactor[Material]; }
	}
}