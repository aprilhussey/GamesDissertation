using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private Transform content;
	[SerializeField]
	private PlayerListContent playerListContentPrefab;

	private List<PlayerListContent> playerListItems = new List<PlayerListContent>();

	[SerializeField]
	private Button playerReadyButton;

	void Awake()
	{
		GetPlayersInCurrentRoom();
	}

	void GetPlayersInCurrentRoom()
	{
		foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
		{
			AddPlayerListItem(player.Value);
		}
	}

	private void AddPlayerListItem(Player newPlayer)
	{
		PlayerListContent playerListItem = Instantiate(playerListContentPrefab, content);
		if (playerListItem != null)
		{
			playerListItem.SetPlayerInfo(newPlayer);
			playerListItems.Add(playerListItem);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		AddPlayerListItem(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		int index = playerListItems.FindIndex(x => x.Player == otherPlayer);
		if (index != -1)    // If found
		{
			Destroy(playerListItems[index].gameObject);
			playerListItems.RemoveAt(index);
		}
	}

	public override void OnLeftRoom()
	{
		//content.DestroyChildren();
		playerListItems.Clear();
	}

	public void OnPlayerReadyClick()
	{
		// Do not use a new hashtable everytime but rather the existing
		// in order to not loose any other properties you might have later
		var hash = PhotonNetwork.LocalPlayer.CustomProperties;
		hash["Ready"] = true;
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		playerReadyButton.gameObject.SetActive(false);

		if (!PhotonNetwork.IsMasterClient) return;

		CheckAllPlayersReady();
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (!PhotonNetwork.IsMasterClient) return;

		if (!changedProps.ContainsKey("Ready")) return;

		CheckAllPlayersReady();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (newMasterClient != PhotonNetwork.LocalPlayer) return;

		CheckAllPlayersReady();
	}

	private void CheckAllPlayersReady()
	{
		var players = PhotonNetwork.PlayerList;

		// Count number of players who are ready
		int readyPlayerCount = players.Count(player => player.CustomProperties.ContainsKey("Ready") && (bool)player.CustomProperties["Ready"]);

		// Check if the number of ready players is equal to the maximum number of players in the room
		if (readyPlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			Debug.Log("All players are ready!");
			GameManager.Instance.ChangeGameScene(GameManager.GameScene.Game);
		}
	}
}
