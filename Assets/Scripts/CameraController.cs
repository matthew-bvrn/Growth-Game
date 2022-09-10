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

	public void StartLooking()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void StopLooking()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void Update()
	{
		if (GameManager.Get.State != GameState.Spectate)
			return;

		if (InputManager.Get.IsJustPressed(EActions.Looking))
			StartLooking();

		if (InputManager.Get.IsJustReleased(EActions.Looking))
			StopLooking();

		float yOffsetFromPivot = transform.position.y - m_pivot.position.y;

		//X rotation
		Vector3 groundPos = new Vector3(transform.position.x, m_pivot.position.y, transform.position.z);
		float radius = Vector3.Distance(groundPos, m_pivot.position);
		float angle = Mathf.Atan2(groundPos.z, groundPos.x);
		float xRotation = InputManager.Get.GetAxis(EActions.RotateX);

		angle -= xRotation * m_xRotateSensitivity;

		Vector3 newPos = radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

		//height
		float heightDelta = -InputManager.Get.GetAxis(EActions.Height) * m_heightSensitivity * yOffsetFromPivot;
		m_height += heightDelta;
		newPos.y = yOffsetFromPivot + heightDelta;

		transform.position = newPos;

		float yRotationDelta = -InputManager.Get.GetAxis(EActions.RotateY) * m_yRotateSensitivity;

		transform.RotateAround(m_pivot.position + new Vector3(0, m_height, 0), Vector3.Cross(groundPos - m_pivot.position,Vector3.up), yRotationDelta);
		transform.rotation= Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.y);

		transform.LookAt(m_pivot.position + new Vector3(0, m_height, 0));

		float zoomIn = InputManager.Get.GetAxis(EActions.ZoomIn);
		float zoomOut = InputManager.Get.GetAxis(EActions.ZoomOut);
		if (zoomIn != 0 || zoomOut != 0)
		{
			float value = zoomIn == 0 ? -zoomOut : zoomIn;

			transform.position = transform.position + transform.forward * value * m_zoomSensitivity * yOffsetFromPivot;
		}
	}
}
