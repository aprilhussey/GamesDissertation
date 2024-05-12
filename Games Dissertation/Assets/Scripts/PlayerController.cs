using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private GameObject currentTileObject;
	private GameObject checkerObjectToMove;

	private GameObject tileObjectToMoveTo;

	private Board board;

	void Awake()
	{
		board = FindObjectOfType<Board>();
	}

	public void OnMouseSelect(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			GameObject hitTile = hit.transform.parent.gameObject;
			Debug.Log($"Raycast hit: {hitTile.transform.name}");

			if (hitTile != null && checkerObjectToMove == null && hitTile.GetComponentInChildren<Checker>())
			{
				currentTileObject = hitTile;
				checkerObjectToMove = currentTileObject.GetComponentInChildren<Checker>().gameObject;

				board.HighlightChecker(currentTileObject.GetComponent<Tile>());

				foreach (List<Tile> row in board.GetBoardList)
				{
					foreach (Tile tile in row)
					{
						if (board.IsValidMove(currentTileObject.GetComponent<Tile>(), tile))
						{
							board.HighlightBoardTile(tile);
						}
					}
				}
			}
			else if (hitTile != null && checkerObjectToMove != null)
			{
				tileObjectToMoveTo = hitTile;

				if (board.BoardTileIsHighlighted(tileObjectToMoveTo.GetComponent<Tile>()))
				{
					board.RemoveHighlightFromChecker();
					board.MoveCheckerWithTiles(currentTileObject.GetComponent<Tile>(), tileObjectToMoveTo.GetComponent<Tile>());

					currentTileObject = null;
					checkerObjectToMove = null;
					tileObjectToMoveTo = null;

					board.RemoveHighlightFromBoardTiles();
					board.RemoveHighlightFromChecker();
				}
			}
		}
	}

	public void OnTouchSelect(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
	}
}
