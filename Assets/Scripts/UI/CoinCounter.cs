using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
	int m_value = 0;

	// Update is called once per frame
	void Update()
	{
		if (m_value != InventoryManager.Get.Money)
		{
			int diff = InventoryManager.Get.Money - m_value;

			m_value += (int)Mathf.Ceil(diff / 30f);

			if (Mathf.Abs(diff) < 30)
				m_value = InventoryManager.Get.Money;
		}
		GetComponent<TMPro.TMP_Text>().text = m_value.ToString();
	}
}
