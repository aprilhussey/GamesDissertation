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

	[SerializeField]
	private Checker checker;
	[SerializeField]
	private GameObject checkerObject;

	private void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();

		if (this.GetComponentInChildren<BoardTile>())
		{
			boardTile = this.GetComponentInChildren<BoardTile>();
		}

		SetCheckerObject();
	}

	public Checker GetChecker
	{
		get { return checker; }
	}

	public GameObject GetCheckerObject
	{
		get { return checkerObject; }
	}

	public void SetCheckerObject()
	{
		if (this.GetComponentInChildren<Checker>())
		{
			checker = this.GetComponentInChildren<Checker>();
			checkerObject = checker.gameObject;
		}
	}

	public void NullCheckerObject()
	{
		checker = null;
		checkerObject = null;
	}
}
