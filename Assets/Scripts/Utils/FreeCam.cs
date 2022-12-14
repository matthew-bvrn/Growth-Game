//Copyright(c) 2019 https://gist.github.com/ashleydavis/f025c03a9221bc840a2b

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A simple free camera to be added to a Unity game object.
/// 
/// Keys:
///	wasd / arrows	- movement
///	q/e 			- up/down (local space)
///	r/f 			- up/down (world space)
///	pageup/pagedown	- up/down (world space)
///	hold shift		- enable fast movement mode
///	right mouse  	- enable free look
///	mouse			- free look / rotation
///     
/// </summary>
public class FreeCam : MonoBehaviour
{
	/// <summary>
	/// Normal speed of camera movement.
	/// </summary>
	public float movementSpeed = 10f;

	/// <summary>
	/// Speed of camera movement when shift is held down,
	/// </summary>
	public float fastMovementSpeed = 100f;

	/// <summary>
	/// Sensitivity for free look.
	/// </summary>
	public float freeLookSensitivity = 3f;

	/// <summary>
	/// Amount to zoom the camera when using the mouse wheel.
	/// </summary>
	public float zoomSensitivity = 10f;

	/// <summary>
	/// Amount to zoom the camera when using the mouse wheel (fast mode).
	/// </summary>
	public float fastZoomSensitivity = 50f;

	/// <summary>
	/// Set to true when free looking (on right mouse button).
	/// </summary>
	private bool looking = false;

	void Update()
	{
		if (StateManager.Get.State != EGameState.Viewing)
			return;

		var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.Q))
		{
			transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.E))
		{
			transform.position = transform.position + (-transform.up * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
		{
			transform.position = transform.position + (Vector3.up * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
		{
			transform.position = transform.position + (-Vector3.up * movementSpeed * Time.deltaTime);
		}

		if (looking)
		{
			float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
			float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
			transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
		}

		float zoomIn = InputManager.Get.GetAxis(EActions.ZoomIn);
		float zoomOut = InputManager.Get.GetAxis(EActions.ZoomOut);
		if (zoomIn != 0 || zoomOut != 0)
		{
			float value = zoomIn == 0 ? -zoomOut : zoomIn;

			var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
			transform.position = transform.position + transform.forward * value * zoomSensitivity;
		}

		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			StartLooking();
		}
		else if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			StopLooking();
		}
	}

	void OnDisable()
	{
		StopLooking();
	}

	/// <summary>
	/// Enable free looking.
	/// </summary>
	public void StartLooking()
	{
		looking = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary>
	/// Disable free looking.
	/// </summary>
	public void StopLooking()
	{
		looking = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}