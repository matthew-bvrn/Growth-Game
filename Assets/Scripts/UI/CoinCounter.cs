using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		GetComponent<TMPro.TMP_Text>().text = InventoryManager.Get.Money.ToString();       
    }
}
