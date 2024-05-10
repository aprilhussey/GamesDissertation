using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

    private bool highlighted = false;

	public BoardTileColor GetBoardTileColor
	{
		get { return boardTileColor; }
	}

	public bool Highlighted
	{
		get { return highlighted; }
		set { highlighted = value; }
	}
}
