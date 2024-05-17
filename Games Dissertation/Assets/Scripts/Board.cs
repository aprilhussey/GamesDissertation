using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
	// Photon
	[HideInInspector]
	public PhotonView photonView;

	private List<List<Tile>> boardList = new List<List<Tile>>();

	[Header("Materials")]
	[SerializeField]
	private Material chessBoardBlack;
	[SerializeField]
	private Material chessFiguresBlack;
	[SerializeField]
	private Material chessFiguresWhite;
	[SerializeField]
	private Material highlight;

	[Header("King Checkers")]
	[SerializeField]
	private GameObject kingCheckerBlackObject;
	[SerializeField]
	private GameObject kingCheckerWhiteObject;

	List<Checker> currentBlackCheckersOnBoard;
	List<Checker> currentWhiteCheckersOnBoard;

	[Header("Win Canvas")]
	[SerializeField]
	private GameObject checkersWinCanvas;

	private List<Tile> totalTilesInBoard;

	void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();

		foreach (Transform child in this.transform)
		{
			Row row = child.GetComponent<Row>();

			if (row != null)
			{
				boardList.Insert(0, row.GetRow);    // Inserts new row at the beginning of the list
			}
		}

		totalTilesInBoard = new List<Tile>();
	}

	void Start()
	{
		foreach (List<Tile> row in boardList)
		{
			foreach (Tile tile in row)
			{
				totalTilesInBoard.Add(tile);
			}
		}

		currentBlackCheckersOnBoard = GetCheckersOnBoard(Checker.CheckerColor.Black);
		currentWhiteCheckersOnBoard = GetCheckersOnBoard(Checker.CheckerColor.White);
	}

	void Update()
	{
		if (checkersHaveNoValidMoves(Checker.CheckerColor.Black))
		{
			CheckersWin(Checker.CheckerColor.White);
		}
		if (checkersHaveNoValidMoves(Checker.CheckerColor.White))
		{
			CheckersWin(Checker.CheckerColor.Black);
		}
	}

	private bool checkersHaveNoValidMoves(Checker.CheckerColor checkerColor)
	{
		List<Tile> tilesNotValidMove = new List<Tile>();

		foreach (List<Tile> row in boardList)
		{
			foreach (Tile tile in row)
			{
				if (tile.GetChecker != null)
				{
					if (tile.GetChecker.GetCheckerColor == checkerColor)
					{
						Tile currentTile = tile;

						foreach (List<Tile> newRow in boardList)
						{
							foreach (Tile newTile in newRow)
							{
								if (!IsValidMove(currentTile, newTile))
								{
									tilesNotValidMove.Add(newTile);
								}
							}
						}
					}
				}
			}
		}

		if (tilesNotValidMove.Count == totalTilesInBoard.Count)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void CheckersWin(Checker.CheckerColor checkerColor)
	{
		TMP_Text checkersWinCanvasText = checkersWinCanvas.GetComponentInChildren<TMP_Text>();
		string originalText = checkersWinCanvasText.text;

		if (checkerColor == Checker.CheckerColor.White)
		{
			checkersWinCanvasText.text = originalText.Replace("COLOR", "WHITE");
			checkersWinCanvas.SetActive(true);
		}
		if (checkerColor == Checker.CheckerColor.Black)
		{
			checkersWinCanvasText.text = originalText.Replace("COLOR", "BLACK");
			checkersWinCanvas.SetActive(true);
		}
	}

	private void CheckerCountWin()
	{
		if (currentBlackCheckersOnBoard.Count < 1)
		{
			//Debug.Log("White checkers win");
			CheckersWin(Checker.CheckerColor.White);
		}
		if (currentWhiteCheckersOnBoard.Count < 1)
		{
			//Debug.Log("Black checkers win");
			CheckersWin(Checker.CheckerColor.Black);
		}
	}

	private void UpdateCheckerCount()
	{
		List<Checker> newBlackCheckersOnBoard = GetCheckersOnBoard(Checker.CheckerColor.Black);
		List<Checker> newWhiteCheckersOnBoard = GetCheckersOnBoard(Checker.CheckerColor.White);

		if (currentBlackCheckersOnBoard.Count > newBlackCheckersOnBoard.Count)
		{
			currentBlackCheckersOnBoard = newBlackCheckersOnBoard;
		}
		if (currentWhiteCheckersOnBoard.Count > newWhiteCheckersOnBoard.Count)
		{
			currentWhiteCheckersOnBoard = newWhiteCheckersOnBoard;
		}
	}

	private List<Checker> GetCheckersOnBoard(Checker.CheckerColor checkerColor)
	{
		List<Checker> checkersOnBoard = new List<Checker>();

		foreach (List<Tile> row in boardList)
		{
			foreach (Tile tile in row)
			{
				if (TileIsNotWhite(tile))
				{
					if (tile.GetChecker != null)
					{
						Checker checkerOnTile = tile.GetChecker;
						if (checkerOnTile.GetCheckerColor == checkerColor)
						{
							checkersOnBoard.Add(checkerOnTile);
						}
					}
				}
			}
		}

		return checkersOnBoard;
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
		for (int y = 0; y < boardList.Count; y++)
		{
			for (int x = 0; x < boardList[y].Count; x++)
			{
				if (boardList[y][x] == tile)
				{
					return (y, x);
				}
			}
		}
		Debug.LogError($"ERROR: Tile position not found");
		return (-1, -1);
	}

	public void SyncMoveChecker((int, int) originalPosition, (int, int) newPosition)
	{
		photonView.RPC("MoveChecker", RpcTarget.AllBufferedViaServer, originalPosition.Item1, originalPosition.Item2, newPosition.Item1, newPosition.Item2);
	}

	[PunRPC]
	public void MoveChecker(int yOriginalPosition, int xOriginalPosition, int yNewPosition, int xNewPosition)
	{
		Tile currentTile = boardList[yOriginalPosition][xOriginalPosition];
		Tile tileToMoveTo = boardList[yNewPosition][xNewPosition];

		currentTile.SetCheckerObject();
		GameObject checkerToMove = currentTile.GetCheckerObject;

		checkerToMove.transform.SetParent(tileToMoveTo.gameObject.transform);
		checkerToMove.transform.localPosition = new Vector3(0, checkerToMove.transform.position.y, 0);

		currentTile.NullCheckerObject();
		tileToMoveTo.SetCheckerObject();
	}

	public void SyncRemoveChecker((int, int) position)
	{
		photonView.RPC("RemoveChecker", RpcTarget.AllBuffered, position.Item1, position.Item2);
	}

	[PunRPC]
	public void RemoveChecker(int y, int x)
	{
		Destroy(boardList[y][x].GetCheckerObject);
		boardList[y][x].NullCheckerObject();

		UpdateCheckerCount();
		CheckerCountWin();
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
		(int y, int x) currentTilePosition = FindTilePosition(currentTile);
		(int y, int x) newTilePosition = FindTilePosition(newTile);

		bool isForwardRelative;

		if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.White)
		{
			return isForwardRelative = currentTilePosition.y < newTilePosition.y;
		}
		else if (currentTile.GetComponentInChildren<Checker>().GetCheckerColor == Checker.CheckerColor.Black)
		{
			return isForwardRelative = currentTilePosition.y > newTilePosition.y;
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

		if (!checker.King)
		{
			Renderer renderer = checker.GetComponent<Renderer>();

			if (renderer != null)
			{
				checker.Highlighted = true;
				renderer.material = highlight;
			}
		}
		else
		{
			checker.Highlighted = true;

			Renderer[] renderers = checker.GetComponentsInChildren<Renderer>();
			if (renderers != null)
			{
				foreach (Renderer renderer in renderers)
				{
					renderer.material = highlight;
				}
			}
		}
	}

	public void RemoveHighlightFromChecker()
	{
		Checker[] checkers = FindObjectsOfType<Checker>();

		foreach (Checker checker in checkers)
		{
			if (checker.Highlighted)
			{
				if (!checker.King)
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
				else
				{
					checker.Highlighted = false;

					Renderer[] renderers = checker.GetComponentsInChildren<Renderer>();
					if (renderers != null)
					{
						foreach (Renderer renderer in renderers)
						{
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
	}

	public bool CheckerReachedOtherSideOfBoard(Tile currentTile, Tile newTile)
	{
		Checker checkerOnTile = currentTile.GetComponentInChildren<Checker>();

		if (checkerOnTile != null)
		{
			switch (checkerOnTile.GetCheckerColor)
			{
				case Checker.CheckerColor.Black:
					for (int i = 0; i <= 7; i++)
					{
						if (FindTilePosition(newTile) == (0, i)) // White side end tiles
						{
							return true;
						}
					}
					return false;
				case Checker.CheckerColor.White:
					for (int i = 0; i <= 7; i++)
					{
						if (FindTilePosition(newTile) == (7, i))   // Black side end tiles
						{
							return true;
						}
					}
					return false;
			}
		}
		return false;
	}

	public void MakeCheckerKing(Tile currentTile, Tile newTile)
	{
		Checker checker = currentTile.GetComponentInChildren<Checker>();

		SyncRemoveChecker(FindTilePosition(newTile));

		switch (checker.GetCheckerColor)
		{
			case Checker.CheckerColor.Black:
				GameObject newKingCheckerBlackObject = PhotonNetwork.Instantiate(kingCheckerBlackObject.name, newTile.gameObject.transform.position, newTile.gameObject.transform.rotation);
				Checker newKingCheckerBlack = newKingCheckerBlackObject.GetComponent<Checker>();
				photonView.RPC("SetTileObjectAsParentOfCheckerObject", RpcTarget.AllBufferedViaServer, newTile.photonView.ViewID, newKingCheckerBlack.photonView.ViewID);
				break;
			case Checker.CheckerColor.White:
				GameObject newKingCheckerWhiteObject = PhotonNetwork.Instantiate(kingCheckerWhiteObject.name, newTile.gameObject.transform.position, newTile.gameObject.transform.rotation);
				Checker newKingCheckerWhite = newKingCheckerWhiteObject.GetComponent<Checker>();
				photonView.RPC("SetTileObjectAsParentOfCheckerObject", RpcTarget.AllBufferedViaServer, newTile.photonView.ViewID, newKingCheckerWhite.photonView.ViewID);
				break;
		}
	}

	[PunRPC]
	public void SetTileObjectAsParentOfCheckerObject(int tileObjectViewID, int checkerObjectViewID)
	{
		GameObject tileObject = PhotonView.Find(tileObjectViewID).gameObject;
		GameObject checkerObject = PhotonView.Find(checkerObjectViewID).gameObject;

		checkerObject.transform.SetParent(tileObject.transform);
		checkerObject.transform.localPosition = new Vector3(0, checkerObject.transform.position.y, 0);

		tileObject.GetComponent<Tile>().SetCheckerObject();

		(int, int) tilePosition = FindTilePosition(tileObject.GetComponent<Tile>());
		Destroy(boardList[tilePosition.Item1][tilePosition.Item2].GetComponentInChildren<Checker>().gameObject);
	}

	public GameObject GetCheckersWinCanvas
	{
		get { return checkersWinCanvas; }
	}
}
