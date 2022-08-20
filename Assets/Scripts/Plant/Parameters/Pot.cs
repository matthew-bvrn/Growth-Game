using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public class Pot
	{
		//TODO make these data driven
		static Dictionary<EPotSize, float> s_potSizeFactor = new Dictionary<EPotSize, float>()
		{
			{EPotSize.Tiny, 20},
			{EPotSize.Small, 50},
			{EPotSize.Medium, 80},
			{EPotSize.Large, 130},
			{EPotSize.Giant, 200},
		};

		static Dictionary<EPotMaterial, float> s_potMaterialDrainingFactor = new Dictionary<EPotMaterial, float>()
		{
			{EPotMaterial.Plastic, 0.8f},
			{EPotMaterial.Terracotta, 1.2f}
		};

		public Pot(EPotSize _size, EPotMaterial _material)
		{
			Size = _size;
			Material = _material;
		}

		public EPotSize Size { get; }
		public EPotMaterial Material { get; }

		internal float GetDrainingFactor()
		{
			return s_potMaterialDrainingFactor[Material];
		}
	}
}