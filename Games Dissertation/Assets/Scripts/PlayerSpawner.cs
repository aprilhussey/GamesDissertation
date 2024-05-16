using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Checker;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    [SerializeField]
    private GameObject whiteOrBlackCanvas;
	[SerializeField]
	private GameObject waitingForRoomOwnerCanvas;

	void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, this.transform.position, this.transform.rotation);

		if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RoomOwnerCheckerColor"))
		{
			Photon.Realtime.Player playerInstance = player.GetComponent<PhotonView>().Owner;

			if (playerInstance == PhotonNetwork.MasterClient)
			{
				Debug.Log($"{playerInstance.NickName} is the Master Client");
				whiteOrBlackCanvas.SetActive(true);
			}
			else
			{
				Debug.Log($"{playerInstance.NickName} is not the Master Client");
				waitingForRoomOwnerCanvas.SetActive(true);
			}
		}
    }

    public void OnWhiteCheckerClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Set the RoomOwnerCheckerColor
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

            customProperties.Add("RoomOwnerCheckerColor", Checker.CheckerColor.White);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
		photonView.RPC("HideCanvases", RpcTarget.AllBuffered);
	}

    public void OnBlackCheckerClick()
    {
		if (PhotonNetwork.IsMasterClient)
		{
			// Set the RoomOwnerCheckerColor
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

			customProperties.Add("RoomOwnerCheckerColor", Checker.CheckerColor.Black);
			PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
		}
		photonView.RPC("HideCanvases", RpcTarget.AllBuffered);
	}

	[PunRPC]
	public void HideCanvases()
    {
		if (whiteOrBlackCanvas.activeInHierarchy)
		{
			whiteOrBlackCanvas.SetActive(false);
		}
		else if (waitingForRoomOwnerCanvas.activeInHierarchy)
		{
			waitingForRoomOwnerCanvas.SetActive(false);
		}
	}

	public GameObject GetWhiteOrBlackCanvas
	{
		get { return whiteOrBlackCanvas; }
	}

	public GameObject GetWaitingForRoomOwnerCanvas
	{
		get { return waitingForRoomOwnerCanvas; }
	}
}
