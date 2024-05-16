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

	public GameObject GetCheckerObject
	{
		get { return checkerObject; }
	}

	public void SetCheckerAndCheckerObject()
	{
		if (this.GetComponentInChildren<Checker>())
		{
			checkerObject = this.GetComponentInChildren<Checker>().gameObject;
		}
	}

	public void NullCheckerAndCheckerObject()
	{
		checkerObject = null;
	}
}
