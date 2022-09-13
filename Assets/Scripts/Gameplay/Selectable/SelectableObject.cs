using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESelectableState
{
	Default,
	Moving
}


public abstract class SelectableObject : MonoBehaviour
{
	internal ESelectableState State {get; set;}


	void Update()
	{
	
	}
}
