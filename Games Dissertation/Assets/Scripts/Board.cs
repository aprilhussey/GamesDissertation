using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	private List<List<Tile>> boardList = new List<List<Tile>>();

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
}
