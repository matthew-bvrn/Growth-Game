using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
#if PC_BUILD
	void Update()
	{
		EGameState state = StateManager.Get.State;
		if (InputManager.Get.IsJustPressed(EActions.OpenInventory))
			StateManager.Get.TrySetState(EGameState.OpenInventoryPressed);

		if (InputManager.Get.IsJustPressed(EActions.CloseMenu))
			StateManager.Get.TrySetState(EGameState.Viewing);
	}

#endif
}
