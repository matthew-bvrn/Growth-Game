using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InspectMenu : MonoBehaviour
{
	[SerializeField] GameObject m_menu;
	[SerializeField] Slider m_healthSlider;
	[SerializeField] Slider m_waterSlider;

	[SerializeField] RawImage m_waterUp;
	[SerializeField] RawImage m_waterDown;
	[SerializeField] RawImage m_healthUp;
	[SerializeField] RawImage m_healthDown;

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
			float health = SelectablesManager.Get.Selected.GetComponent<Parameters.ParametersComponent>().GrowthFactor;
			float water = SelectablesManager.Get.Selected.GetComponent<Parameters.ParametersComponent>().WaterLevel;

			UpdateArrows(m_waterSlider.value, water, m_waterUp, m_waterDown);
			UpdateArrows(m_healthSlider.value, health, m_healthUp, m_healthDown);

			m_healthSlider.value = health;
			m_waterSlider.value = water;


		}
	}

	void UpdateArrows(float oldVal, float newVal, RawImage up, RawImage down)
	{
		if (newVal > oldVal)
		{
			up.gameObject.SetActive(true);
			down.gameObject.SetActive(false);
		}
		else if(newVal < oldVal)
		{
			up.gameObject.SetActive(false);
			down.gameObject.SetActive(true);
		}
		else
		{
			up.gameObject.SetActive(false);
			down.gameObject.SetActive(false);
		}
	}

	public void OnWaterPressed()
	{
		SelectablesManager.Get.Selected.GetComponent<GrowthComponent>().Water();
	}

}
