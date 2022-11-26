using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDisable : MonoBehaviour
{
	public void Start()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		Debug.Log("hi");
		EventSystem.current.SetSelectedGameObject(null);
	}
}
