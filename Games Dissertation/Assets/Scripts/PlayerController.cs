using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private GameObject currentTile;
	private GameObject checkerToMove;

	private GameObject tileToMoveTo;

	public void OnMouseSelect(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			GameObject hitTile = hit.transform.parent.gameObject;
			Debug.Log($"Raycast hit: {hitTile.transform.name}");

			if (hitTile != null && checkerToMove == null && hitTile.GetComponentInChildren<Checker>())
			{
				currentTile = hitTile;
				checkerToMove = currentTile.GetComponentInChildren<Checker>().gameObject;
			}
			else if (hitTile != null && checkerToMove != null && !hitTile.GetComponentInChildren<Checker>())
			{
				tileToMoveTo = hitTile;

				Board board = FindObjectOfType<Board>();

				board.MoveCheckerWithTiles(currentTile.GetComponent<Tile>(), tileToMoveTo.GetComponent<Tile>());
				
				currentTile = null;
				checkerToMove = null;
				tileToMoveTo = null;
			}
		}
	}

	public void OnTouchSelect(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
	}
}
