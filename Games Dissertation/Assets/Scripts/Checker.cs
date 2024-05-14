using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
	public enum CheckerColor
	{
		Black,
		White
	}

	// Photon
	[HideInInspector]
	public PhotonView photonView;

	[SerializeField]
	private CheckerColor checkerColor;

	[SerializeField]
	private bool king;

	private bool highlighted = false;

	void Awake()
	{
		// Photon
		photonView = this.GetComponent<PhotonView>();
	}

	public CheckerColor GetCheckerColor
	{
		get { return checkerColor; }
	}

	public bool King
	{
		get { return king; }
		set { king = value; }
	}

	public bool Highlighted
	{
		get { return highlighted; }
		set { highlighted = value; }
	}
}
