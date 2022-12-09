using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : UiItemScrollView
{
	protected override EGameState GetState()
	{
		return EGameState.ShopOpen;
	}
}
