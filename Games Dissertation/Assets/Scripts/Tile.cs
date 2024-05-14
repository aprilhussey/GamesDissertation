using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	// Photon
	[HideInInspector]
	public PhotonView photonView;

    private GameObject boardTileObject;
    private GameObject checkerObject;

	private void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();

		if (this.GetComponentInChildren<BoardTile>())
		{
			boardTileObject = this.GetComponentInChildren<BoardTile>().gameObject;
		}

		SetCheckerObject();
	}

	public GameObject GetCheckerObject()
	{
		return checkerObject;
	}

	public void SetCheckerObject()
	{
		if (this.GetComponentInChildren<Checker>())
		{
			checkerObject = this.GetComponentInChildren<Checker>().gameObject;
		}
	}

	public void NullCheckerObject()
	{
		if (checkerObject != null)
		{
			checkerObject = null;
		}
	}
}
