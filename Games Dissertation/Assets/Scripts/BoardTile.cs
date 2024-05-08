using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private enum BoardTileColor
    {
        black,
        white
    }

    [SerializeField]
    private BoardTileColor boardTileColor;
}
