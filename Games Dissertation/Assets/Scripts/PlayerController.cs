using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputActionAsset inputActionAsset;
	private InputActionMap playerActionMap;

	private Vector2 screenPosition;

	private Camera mainCamera;
	private Board board;

	private GameObject currentTileObject;
	private GameObject checkerObjectToMove;

	private GameObject tileObjectToMoveTo;

	void Awake()
    {
		playerInput = this.GetComponent<PlayerInput>();

		inputActionAsset = playerInput.actions;
		playerActionMap = inputActionAsset.FindActionMap("Player");

		inputActionAsset.Enable();

		mainCamera = Camera.main;
		board = FindObjectOfType<Board>();
	}

    void OnEnable()
    {
		playerActionMap["Press"].performed += OnPress;
	}

    void OnDisable()
    {
		playerActionMap["Press"].performed -= OnPress;
	}

	private void OnPress(InputAction.CallbackContext context)
    {
		screenPosition = playerActionMap["ScreenPosition"].ReadValue<Vector2>();
		//Debug.Log($"Screen position: {screenPosition}");

		Ray ray = mainCamera.ScreenPointToRay(screenPosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.GetComponentInParent<Tile>() != null)
			{
				GameObject hitTile = hit.transform.GetComponentInParent<Tile>().gameObject;

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
				else if (hitTile == currentTileObject && checkerObjectToMove != null)
				{
					ResetVariables();
				}
				else if (hitTile != null && checkerObjectToMove != null)
				{
					tileObjectToMoveTo = hitTile;

					if (board.BoardTileIsHighlighted(tileObjectToMoveTo.GetComponent<Tile>()))
					{
						board.MoveCheckerWithTiles(currentTileObject.GetComponent<Tile>(), tileObjectToMoveTo.GetComponent<Tile>());
						board.HoppedChecker(currentTileObject.GetComponent<Tile>(), tileObjectToMoveTo.GetComponent<Tile>());

						ResetVariables();
					}
				}
			}
		}
	}

	private void ResetVariables()
	{
		currentTileObject = null;
		checkerObjectToMove = null;
		tileObjectToMoveTo = null;

		board.RemoveHighlightFromBoardTiles();
		board.RemoveHighlightFromChecker();
	}
}

