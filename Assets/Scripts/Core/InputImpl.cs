using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface InputImpl
{
	void Initialise(ref Dictionary<EActions, InputBase> inputs);

	void UpdateInputs(ref Dictionary<EActions, InputBase> inputs);
}
