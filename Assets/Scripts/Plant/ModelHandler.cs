using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ModelHandler : SimulatableBase
{
	internal abstract override void Simulate(float delta);
}
