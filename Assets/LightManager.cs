using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		GameConsole.Instance.AddCommand("settime", DebugSetTime);
	}

	void DebugSetTime(int count, params string[] args)
	{
		if(args[0] == "day")
		{
			RenderSettings.ambientSkyColor = new Color(231/255f, 211 / 255f, 199 / 255f);
			//RenderSettings.ambientIntensity = 0;			
		}

		if(args[0] == "night")
		{
			RenderSettings.ambientSkyColor = new Color(31 / 255f, 37 / 255f, 65 / 255f);
		//	RenderSettings.ambientIntensity = 0;
		}
	}
}
