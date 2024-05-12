using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
	private List<List<Tile>> boardList = new List<List<Tile>>();

	[SerializeField]
	private Material chessBoardBlack;

	[SerializeField]
	private Material tileHighlight;

	void Awake()
	{
		foreach (Transform child in this.transform)
		{
			Row row = child.GetComponent<Row>();

			if (row != null)
			{
				boardList.Add(row.GetRow);
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
		MoveChecker(currentTilePosition, newTilePosition);
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

	public void MoveChecker((int, int) originalPosition, (int, int) newPosition)
	{
		GameObject currentTile = boardList[originalPosition.Item1][originalPosition.Item2].gameObject;
		GameObject checkerToMove = currentTile.GetComponent<Tile>().GetCheckerObject();
		GameObject tileToMoveTo = boardList[newPosition.Item1][newPosition.Item2].gameObject;

		checkerToMove.transform.SetParent(tileToMoveTo.transform);
		checkerToMove.transform.localPosition = new Vector3(0, checkerToMove.transform.position.y, 0);

		currentTile.GetComponent<Tile>().NullCheckerObject();
		tileToMoveTo.GetComponent<Tile>().SetCheckerObject();
	}

	public void RemoveChecker((int, int) position)
	{
		Destroy(boardList[position.Item1][position.Item2].GetCheckerObject());
		boardList[position.Item1][position.Item2].NullCheckerObject();
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
		if (tile.GetComponentInChildren<BoardTile>().GetBoardTileColor != BoardTile.BoardTileColor.white)
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

		if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.white)
		{
			return isForwardRelative = currentTilePosition.x > newTilePosition.x;
		}
		else if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.black)
		{
			return isForwardRelative = currentTilePosition.x < newTilePosition.x;
		}
		else
		{
			return false;
		}
	}

	private bool TileIsDiagonal(Tile currentTile, Tile newTile)
	{
		(int x, int y) currentTilePosition = FindTilePosition(currentTile);
		(int x, int y) newTilePosition = FindTilePosition(newTile);

		int xDiagonal = Math.Abs(currentTilePosition.x - newTilePosition.x);
		int yDiagonal = Math.Abs(currentTilePosition.y - newTilePosition.y);

		return xDiagonal == yDiagonal;
	}

	private bool TileIsValidDiagonalMove(Tile currentTile, Tile newTile)
	{
		(int x, int y) currentTilePosition = FindTilePosition(currentTile);
		(int x, int y) newTilePosition = FindTilePosition(newTile);

		int xDiagonal = Math.Abs(currentTilePosition.x - newTilePosition.x);
		int yDiagonal = Math.Abs(currentTilePosition.y - newTilePosition.y);

		if (xDiagonal == yDiagonal)
		{
			// Check if move is only one tile away
			if (xDiagonal == 1)
			{
				return true;
			}
			// Check if move is two tiles away and there is a checker to hop over
			else if (xDiagonal == 2)
			{
				Tile middleTile = boardList[(currentTilePosition.x + newTilePosition.x) / 2][(currentTilePosition.y + newTilePosition.y / 2)];
				if (middleTile.GetComponentInChildren<Checker>() != null)
				{
					return true;
				}
			}
		}
		return false;
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
			renderer.material = tileHighlight;
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
}
