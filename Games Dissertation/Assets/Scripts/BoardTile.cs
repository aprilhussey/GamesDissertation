using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Checker;

public class BoardTile : MonoBehaviour
{
    public enum BoardTileColor
    {
        black,
        white
    }

    [SerializeField]
    private BoardTileColor boardTileColor;

	public BoardTileColor GetBoardTileColor
	{
		get { return boardTileColor; }
	}
}
