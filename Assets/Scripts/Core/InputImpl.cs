using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface InputImpl
{
	void Initialise(ref Dictionary<EActions, InputBase> inputs);
	Vector2 GetSelectionPosition();
}
