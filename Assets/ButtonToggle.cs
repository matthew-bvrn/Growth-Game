using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonToggle : MonoBehaviour
{
	bool m_state = false;

	public void Start()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		m_state = !m_state;

		if(!m_state)
			EventSystem.current.SetSelectedGameObject(null);
	}
}
