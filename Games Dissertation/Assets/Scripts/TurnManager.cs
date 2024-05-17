using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviourPunCallbacks
{
	// The player whose turn it is
	public Checker.CheckerColor currentPlayer;

	[SerializeField]
	private GameObject whiteCheckersTurn;
	[SerializeField]
	private GameObject blackCheckersTurn;

	void Start()
	{
		currentPlayer = Checker.CheckerColor.White;
		UpdateCurrentPlayerCanvas(currentPlayer);
	}

	// Call this method to switch to the next player's turn
	public void NextTurn()
	{
		currentPlayer = (currentPlayer == 
			Checker.CheckerColor.White) ? 
			Checker.CheckerColor.Black : 
			Checker.CheckerColor.White;

		// Synchronize the current player across all clients
		photonView.RPC("SyncCurrentPlayer", RpcTarget.AllBuffered, (int)currentPlayer);
	}

	[PunRPC]
	public void SyncCurrentPlayer(int player)
	{
		currentPlayer = (Checker.CheckerColor)player;
		UpdateCurrentPlayerCanvas(currentPlayer);
	}

	public void UpdateCurrentPlayerCanvas(Checker.CheckerColor currentPlayer)
	{
		if (currentPlayer == Checker.CheckerColor.White)
		{
			whiteCheckersTurn.SetActive(true);
			blackCheckersTurn.SetActive(false);
		}
		if (currentPlayer == Checker.CheckerColor.Black)
		{
			blackCheckersTurn.SetActive(true);
			whiteCheckersTurn.SetActive(false);
		}
	}
}
