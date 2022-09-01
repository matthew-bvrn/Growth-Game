using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract internal class ModelHandler : ISimulatable, IAgeable
{
	internal abstract override void Simulate(float growth, float deltaGrowth);
	public abstract void Age(float deltaSeconds);
}
