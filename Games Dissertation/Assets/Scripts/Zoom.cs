using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
	[SerializeField]
	private GameObject mainCamera;

	[SerializeField]
	private float zoomSpeed = 5f;

	private float xAngle;
	private float yAngle;
	private float zAngle;

	private void Awake()
	{
		xAngle = mainCamera.transform.rotation.eulerAngles.x;
		yAngle = mainCamera.transform.rotation.eulerAngles.y;
		zAngle = mainCamera.transform.rotation.eulerAngles.z;

		mainCamera.transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
	}

	public void OnMouseZoom(InputAction.CallbackContext context)
	{
		float zoom = context.ReadValue<Vector2>().y;

		// Set rotation of camera
		Quaternion rotation = Quaternion.Euler(xAngle, yAngle, zAngle);

		// Calculate the forward direction based on the rotation
		Vector3 calculatedForward = rotation * Vector3.forward;

		// Zoom happens along the calculated forward direction
		Vector3 movement = zoom * zoomSpeed * Time.deltaTime * calculatedForward;
		this.transform.Translate(movement, Space.Self);
	}

	public void OnTouchZoom(InputAction.CallbackContext context)
	{

	}
}