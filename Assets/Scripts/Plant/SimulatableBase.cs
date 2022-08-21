using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimulatableBase : MonoBehaviour
{
	internal abstract void Simulate(float deltaSeconds);
}
