using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ModelHandler : ISimulatable
{
	internal abstract override void Simulate(float growth, float deltaGrowth);
}
