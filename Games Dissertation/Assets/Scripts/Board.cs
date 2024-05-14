using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	// Photon
	[HideInInspector]
	public PhotonView photonView;

	private List<List<Tile>> boardList = new List<List<Tile>>();

	[SerializeField]
	private Material chessBoardBlack;

	[SerializeField]
	private Material chessFiguresBlack;

	[SerializeField]
	private Material chessFiguresWhite;

	[SerializeField]
	private Material highlight;

	void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();

		foreach (Transform child in this.transform)
		{
			Row row = child.GetComponent<Row>();

			if (row != null)
			{
				boardList.Insert(0, row.GetRow);	// Inserts new row at the beginning of the list
			}
		}
	}

	public List<List<Tile>> GetBoardList
	{
		get { return boardList; }
	}

	public void MoveCheckerWithTiles(Tile currentTile, Tile newTile)
	{
		(int, int) currentTilePosition = FindTilePosition(currentTile);
		(int, int) newTilePosition = FindTilePosition(newTile);
		SyncMoveChecker(currentTilePosition, newTilePosition);
	}

	public (int, int) FindTilePosition(Tile tile)
	{
		for (int i = 0; i < boardList.Count; i++)
		{
			for (int j = 0; j < boardList[i].Count; j++)
			{
				if (boardList[i][j] == tile)
				{
					return (i, j);
				}
			}
		}
		Debug.LogError($"ERROR: Tile position not found");
		return (-1, -1);
	}

	public void SyncMoveChecker((int, int) originalPosition, (int, int) newPosition)
	{
		photonView.RPC("MoveChecker", RpcTarget.All, originalPosition, newPosition);
	}

	[PunRPC]
	public void MoveChecker(Vector2Int originalPosition, Vector2Int newPosition)
	{
		GameObject currentTile = boardList[originalPosition.x][originalPosition.y].gameObject;
		GameObject checkerToMove = currentTile.GetComponent<Tile>().GetCheckerObject();
		GameObject tileToMoveTo = boardList[newPosition.x][newPosition.y].gameObject;

		checkerToMove.transform.SetParent(tileToMoveTo.transform);
		checkerToMove.transform.localPosition = new Vector3(0, checkerToMove.transform.position.y, 0);

		currentTile.GetComponent<Tile>().NullCheckerObject();
		tileToMoveTo.GetComponent<Tile>().SetCheckerObject();
	}

	public void SyncRemoveChecker((int, int) position)
	{
		photonView.RPC("RemoveChecker", RpcTarget.All, position);
	}

	[PunRPC]
	public void RemoveChecker(Vector2Int position)
	{
		Destroy(boardList[position.x][position.y].GetCheckerObject());
		boardList[position.x][position.y].NullCheckerObject();
	}

	public bool IsValidMove(Tile currentTile, Tile newTile)
	{
		if (TileIsNotWhite(newTile))
		{
			if (TileDoesNotContainChecker(newTile))
			{
				if (currentTile.GetComponentInChildren<Checker>().King == false)
				{
					if (TileIsForwardRelative(currentTile, newTile))
					{
						if (TileIsValidDiagonalMove(currentTile, newTile))
						{
							return true;
						}
					}
				}
				else if (currentTile.GetComponentInChildren<Checker>().King == true)
				{
					if (TileIsValidDiagonalMove(currentTile, newTile))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool TileIsNotWhite(Tile tile)
	{
		if (tile.GetComponentInChildren<BoardTile>().GetBoardTileColor != BoardTile.BoardTileColor.White)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool TileDoesNotContainChecker(Tile tile)
	{
		if (tile.GetComponentInChildren<Checker>() == null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool TileIsForwardRelative(Tile currentTile, Tile newTile)
	{
		(int x, int y) currentTilePosition = FindTilePosition(currentTile);
		(int x, int y) newTilePosition = FindTilePosition(newTile);

		bool isForwardRelative;

		if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.White)
		{
			return isForwardRelative = currentTilePosition.x < newTilePosition.x;
		}
		else if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.Black)
		{
			return isForwardRelative = currentTilePosition.x > newTilePosition.x;
		}
		else
		{
			return false;
		}
	}

	public bool TileIsValidDiagonalMove(Tile currentTile, Tile newTile)
	{
		(int y, int x) currentTilePosition = FindTilePosition(currentTile);
		(int y, int x) newTilePosition = FindTilePosition(newTile);

		int xDiagonal = Math.Abs(currentTilePosition.x - newTilePosition.x);
		int yDiagonal = Math.Abs(currentTilePosition.y - newTilePosition.y);

		if (xDiagonal == yDiagonal)
		{
			// Check if move is only one tile away
			if (yDiagonal == 1)
			{
				return true;
			}
			// Check if move is two tiles away and there is a checker to hop over
			else if (yDiagonal == 2)
			{
				Tile middleTile = boardList[(currentTilePosition.y + newTilePosition.y) / 2][(currentTilePosition.x + newTilePosition.x) / 2];
				if (middleTile.GetComponentInChildren<Checker>() != null)
				{
					if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor
						!= middleTile.GetComponentInChildren<Checker>().GetCheckerColor)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void HoppedChecker(Tile currentTile, Tile newTile)
	{
		(int y, int x) currentTilePosition = FindTilePosition(currentTile);
		(int y, int x) newTilePosition = FindTilePosition(newTile);

		int xDiagonal = Math.Abs(currentTilePosition.x - newTilePosition.x);
		int yDiagonal = Math.Abs(currentTilePosition.y - newTilePosition.y);

		if (xDiagonal == yDiagonal)
		{
			// Check if move is two tiles away and there is a checker to hop over
			if (yDiagonal == 2)
			{
				Tile middleTile = boardList[(currentTilePosition.y + newTilePosition.y) / 2][(currentTilePosition.x + newTilePosition.x) / 2];
				if (middleTile.GetComponentInChildren<Checker>() != null)
				{
					SyncRemoveChecker(FindTilePosition(middleTile));
				}
			}
		}
	}

	public bool BoardTileIsHighlighted(Tile tile)
	{
		return tile.GetComponentInChildren<BoardTile>().Highlighted;
	}

	public void HighlightBoardTile(Tile tile)
	{
		BoardTile boardTile = tile.GetComponentInChildren<BoardTile>();
		Renderer renderer = boardTile.GetComponent<Renderer>();

		if (renderer != null)
		{
			boardTile.Highlighted = true;
			renderer.material = highlight;
		}
	}

	public void RemoveHighlightFromBoardTiles()
	{
		BoardTile[] boardTiles = FindObjectsOfType<BoardTile>();

		foreach (BoardTile boardTile in boardTiles)
		{
			if (boardTile.Highlighted)
			{
				Renderer renderer = boardTile.GetComponent<Renderer>();

				if (renderer != null)
				{
					boardTile.Highlighted = false;
					renderer.material = chessBoardBlack;
				}
			}
		}
	}

	public void HighlightChecker(Tile tile)
	{
		Checker checker = tile.GetComponentInChildren<Checker>();
		Renderer renderer = checker.GetComponent<Renderer>();

		if (renderer != null)
		{
			checker.Highlighted = true;
			renderer.material = highlight;
		}
	}

	public void RemoveHighlightFromChecker()
	{
		Checker[] checkers = FindObjectsOfType<Checker>();

		foreach (Checker checker in checkers)
		{
			if (checker.Highlighted)
			{
				Renderer renderer = checker.GetComponent<Renderer>();

				if (renderer != null)
				{
					checker.Highlighted = false;

					if (checker.GetCheckerColor == Checker.CheckerColor.Black)
					{
						renderer.material = chessFiguresBlack;
					}
					else if (checker.GetCheckerColor == Checker.CheckerColor.White)
					{
						renderer.material = chessFiguresWhite;
					}
				}
			}
		}
	}
}
