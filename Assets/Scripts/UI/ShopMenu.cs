using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : ItemMenu
{
	[SerializeField] GameObject m_price;

	protected override EGameState GetState()
	{
		return EGameState.ShopOpen;
	}

	protected override void AdditionalOnActivated()
	{
		m_price.SetActive(false);
	}

	public override void OnElementClicked()
	{
		base.OnElementClicked();

		m_price.SetActive(true);
		m_price.GetComponentInChildren<TMPro.TMP_Text>().text = m_selected.Price.ToString();

	}
}
