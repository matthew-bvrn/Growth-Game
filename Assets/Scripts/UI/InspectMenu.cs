using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InspectMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;
	[SerializeField] Slider m_healthSlider;

	public void OnInspect()
	{
		StateManager.Get.TrySetState(EGameState.Inspecting);
	}

	private void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	void OnStateChanged(EGameState state)
	{
		if (state == EGameState.Inspecting)
		{
			m_menu.SetActive(true);
		}
		else
		{
			m_menu.SetActive(false);
		}
	}

	private void Update()
	{
		if(StateManager.Get.State == EGameState.Inspecting)
		{
			Debug.Log(SelectablesManager.Get.Selected.GetComponent<Parameters.ParametersComponent>().WaterHealth);
			m_healthSlider.value = SelectablesManager.Get.Selected.GetComponent<Parameters.ParametersComponent>().GrowthFactor;
		}
	}

}
