using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameObject boardTile;
    private GameObject checker;

	private void Awake()
	{
		boardTile = this.GetComponentInChildren<BoardTile>().gameObject;
		checker = this.GetComponentInChildren<Checker>().gameObject;
	}
}
