using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
	[SerializeField]
	private GameObject mainCamera;

	[SerializeField]
	private float zoomSpeed = 10f;

	[SerializeField]
	private float xAngle = 45f;
	private float yAngle = 0f;
	private float zAngle = 0f;

	private void Awake()
	{
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