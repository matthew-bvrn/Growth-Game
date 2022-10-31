using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectMenu : MonoBehaviour
{
   public void OnInspect()
	{
		StateManager.Get.TrySetState(EGameState.Inspecting);
	}
}
