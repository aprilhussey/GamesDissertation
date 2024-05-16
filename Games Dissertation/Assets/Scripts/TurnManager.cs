using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviourPunCallbacks
{
	// The player whose turn it is
	public Checker.CheckerColor currentPlayer;

	void Start()
	{
		currentPlayer = Checker.CheckerColor.White;
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
	void SyncCurrentPlayer(int player)
	{
		currentPlayer = (Checker.CheckerColor)player;
	}
}
