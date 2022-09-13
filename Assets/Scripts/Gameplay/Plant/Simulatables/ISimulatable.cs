using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitParamsBase { }

public abstract class ISimulatable : MonoBehaviour
{
	public virtual void Initialise(InitParamsBase initParams) => IsInitialised = true;
	//Anything affected by the growth
	internal virtual void Simulate(float growth, float deltaGrowth) { }
	//Anything that affects the growth
	internal virtual void PreSimulate(float deltaSeconds) { }

	public ISimulatable() => IsInitialised = false;

	protected bool CheckInitialistion()
	{
		if (!IsInitialised)
		{
			Debug.LogError("Component not initialised, returning");
			return false;
		}

		return true;
	}

	public bool IsInitialised { get; private set; }
}
