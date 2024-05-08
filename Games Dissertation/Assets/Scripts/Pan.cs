using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pan : MonoBehaviour
{
	[SerializeField]
	private GameObject mainCamera;

	[SerializeField]
	private float panSpeed = 5f;

	public void OnMousePan(InputAction.CallbackContext context)
	{
		Vector2 delta = context.ReadValue<Vector2>();

		Vector3 movement = new Vector3(-delta.x, 0, -delta.y) * panSpeed * Time.deltaTime;  // Pan affects the x and z axis
		this.transform.Translate(movement, Space.World);
	}

	public void OnTouchPan(InputAction.CallbackContext context)
	{

	}
}
