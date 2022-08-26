using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitParamsBase { }

public abstract class SimulatableBase : MonoBehaviour
{
	public virtual void Initialise(InitParamsBase initParams) => IsInitialised = true;
	internal abstract void Simulate(float deltaSeconds);

	public SimulatableBase() => IsInitialised = false;

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
