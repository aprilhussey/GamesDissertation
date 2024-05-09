using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	private List<List<Tile>> board = new List<List<Tile>>();

	void Awake()
	{
		foreach (Transform child in this.transform)
		{
			Row row = child.GetComponent<Row>();

			if (row != null)
			{
				board.Add(row.GetRow);
			}
		}
	}

	public List<List<Tile>> GetBoard
	{
		get { return board; }
	}

	void MoveChecker((int, int) originalPosition, (int, int) newPosition)
	{
		GameObject currentTile = board[originalPosition.Item1][originalPosition.Item2].gameObject;
		GameObject checkerToMove = currentTile.GetComponent<Tile>().GetCheckerObject();
		GameObject tileToMoveTo = board[newPosition.Item1][newPosition.Item2].gameObject;

		checkerToMove.transform.SetParent(tileToMoveTo.transform);
		currentTile.GetComponent<Tile>().NullCheckerObject();
		tileToMoveTo.GetComponent<Tile>().SetCheckerObject();
	}

	void RemoveChecker((int, int) position)
	{
		Destroy(board[position.Item1][position.Item2].GetCheckerObject());
		board[position.Item1][position.Item2].NullCheckerObject();
	}
}
