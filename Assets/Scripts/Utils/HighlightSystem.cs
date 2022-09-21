using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSystem : MonoBehaviour
{
	public static HighlightSystem Get;

	public bool ElementHighlighted { get; private set; } = false;

	// Start is called before the first frame update
	void Start()
	{
		if (Get == null)
		{
			Get = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public void AddHighlighted()
	{
		ElementHighlighted = true;
	}

	public void RemoveHighlighted()
	{
		ElementHighlighted = false;
	}
}
