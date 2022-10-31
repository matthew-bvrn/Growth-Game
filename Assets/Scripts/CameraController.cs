//Portions of this script Copyright(c) 2019 https://gist.github.com/ashleydavis/f025c03a9221bc840a2b

//	Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] float m_height = 0;

	[SerializeField] float m_xRotateSensitivity = 0.1f;
	[SerializeField] float m_yRotateSensitivity = 3.5f;
	[SerializeField] float m_zoomSensitivity = 3f;
	[SerializeField] float m_heightSensitivity = 0.1f;
	[SerializeField] Transform m_pivot;

	Vector3 m_prevCamPos;
	Vector3 m_newCamPos;
	Quaternion m_prevCamRot;
	Quaternion m_newCamRot;
	float m_cameraMoveTime = 0.5f;
	float m_selectedCamProgress = 1f;

	public void Start()
	{
		StateManager.Get.OnStateChange += OnStateChanged;
	}

	public void OnStateChanged(EGameState state)
	{
		if (state == EGameState.CameraMoving)
			StartMoving();
		else if (StateManager.Get.PreviousState == EGameState.CameraMoving)
			StopMoving();
		else if (state == EGameState.Inspecting)
		{
			Vector3 offset = Quaternion.Euler(0, 90, 0) * transform.rotation * new Vector3(2, 0, 0);
			Vector3 newCamPos = SelectablesManager.Get.Selected.transform.position + new Vector3(0f, 2, 0) + offset;
			Quaternion newCamRot= Quaternion.Euler(35, transform.eulerAngles.y, 0);

			InitiateMotion(newCamPos, newCamRot);
		}
		else if (state == EGameState.Viewing && StateManager.Get.PreviousState == EGameState.Inspecting)
		{
			InitiateMotion(m_prevCamPos, m_prevCamRot);
		}
	}

	public void StartMoving()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void StopMoving()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void InitiateMotion(Vector3 newCamPos, Quaternion newCamRot, float cameraMoveTime = 0.5f)
	{
		m_newCamPos = newCamPos;
		m_newCamRot = newCamRot;
		m_cameraMoveTime = cameraMoveTime;

		m_prevCamPos = transform.position;
		m_prevCamRot = transform.rotation;
		m_selectedCamProgress = 0;
	}

	void UpdateMotion()
	{
		if (m_selectedCamProgress < 1)
		{
			transform.position = m_newCamPos * m_selectedCamProgress + m_prevCamPos * (1 - m_selectedCamProgress);
			transform.rotation = Quaternion.Lerp(m_prevCamRot, m_newCamRot, m_selectedCamProgress);

			m_selectedCamProgress += Time.deltaTime / m_cameraMoveTime;
		}
	}

	void Update()
	{
		EGameState state = StateManager.Get.State;

		UpdateMotion();

		if (state == EGameState.Inspecting)
		{
			transform.RotateAround(SelectablesManager.Get.Selected.transform.position, Vector3.up, 3 * Time.deltaTime);
		}

		if (state != EGameState.Viewing && state != EGameState.CameraMoving)
			return;

		if (InputManager.Get.IsJustPressed(EActions.CameraMoving))
			StateManager.Get.TrySetState(EGameState.CameraMoving);

		if (InputManager.Get.IsJustReleased(EActions.CameraMoving))
			StateManager.Get.TrySetState(EGameState.Viewing);

		float yOffsetFromPivot = transform.position.y - m_pivot.position.y;

		//prevents camera moving when we keep the moving button held down from another state
		if (state == EGameState.CameraMoving)
		{
			//X rotation
			Vector3 groundPos = new Vector3(transform.position.x, m_pivot.position.y, transform.position.z);
			float radius = Vector3.Distance(groundPos, m_pivot.position);
			float angle = Mathf.Atan2(groundPos.z, groundPos.x);
			float xRotation = InputManager.Get.GetAxis(EActions.RotateX);

			angle -= xRotation * m_xRotateSensitivity;

			Vector3 newPos = radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

			//height
			float heightDelta = -InputManager.Get.GetAxis(EActions.ChangeHeight) * m_heightSensitivity * yOffsetFromPivot;
			m_height += heightDelta;
			newPos.y = yOffsetFromPivot + heightDelta;

			transform.position = newPos;

			float yRotationDelta = -InputManager.Get.GetAxis(EActions.RotateY) * m_yRotateSensitivity;

			transform.RotateAround(m_pivot.position + new Vector3(0, m_height, 0), Vector3.Cross(groundPos - m_pivot.position, Vector3.up), yRotationDelta);
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.y);

			transform.LookAt(m_pivot.position + new Vector3(0, m_height, 0));
		}

		float zoomIn = InputManager.Get.GetAxis(EActions.ZoomIn);
		float zoomOut = InputManager.Get.GetAxis(EActions.ZoomOut);
		if (zoomIn != 0 || zoomOut != 0)
		{
			StateManager.Get.TrySetState(EGameState.Viewing);

			float value = zoomIn == 0 ? -zoomOut : zoomIn;

			transform.position = transform.position + transform.forward * value * m_zoomSensitivity * yOffsetFromPivot;
		}
	}
}
