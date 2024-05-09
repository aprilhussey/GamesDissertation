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
}
