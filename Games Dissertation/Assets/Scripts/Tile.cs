using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameObject boardTileObject;
    private GameObject checkerObject;

	private void Awake()
	{
		if (this.GetComponentInChildren<BoardTile>())
		{
			boardTileObject = this.GetComponentInChildren<BoardTile>().gameObject;
		}

		SetCheckerObject();
	}

	public void SetCheckerObject()
	{
		if (this.GetComponentInChildren<Checker>())
		{
			checkerObject = this.GetComponentInChildren<Checker>().gameObject;
		}
	}
}
