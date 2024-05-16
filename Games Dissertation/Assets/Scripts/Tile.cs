using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	// Photon
	[HideInInspector]
	public PhotonView photonView;

    private BoardTile boardTile;

	private Checker checker;
	private GameObject checkerObject;

	private void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();

		if (this.GetComponentInChildren<BoardTile>())
		{
			boardTile = this.GetComponentInChildren<BoardTile>();
		}

		SetCheckerAndCheckerObject();
	}

	public Checker GetChecker
	{
		get { return checker; }
	}

	public GameObject GetCheckerObject
	{
		get { return checkerObject; }
	}

	public void SetCheckerAndCheckerObject()
	{
		if (this.GetComponentInChildren<Checker>())
		{
			checker = this.GetComponentInChildren<Checker>();
			checkerObject = checker.gameObject;
		}
	}

	public void NullCheckerObject()
	{
		if (checkerObject != null)
		{
			checker = null;
			checkerObject = null;
		}
	}
}
