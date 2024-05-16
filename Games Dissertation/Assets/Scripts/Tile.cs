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

		SetCheckerObject();
	}

	public GameObject GetCheckerObject
	{
		get { return checkerObject; }
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
		checkerObject = null;
	}
}
