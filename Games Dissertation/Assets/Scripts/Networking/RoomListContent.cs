using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListContent : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI roomNameText;
	[SerializeField]
	private TextMeshProUGUI currentPlayersText;
	[SerializeField]
	private TextMeshProUGUI maxPlayersText;

	public RoomInfo RoomInfo { get; private set; }

	private CreateOrJoinRoom createOrJoinRoom;

	private void Awake()
	{
		createOrJoinRoom = GameObject.FindObjectOfType<CreateOrJoinRoom>();
	}

	public void OnJoinRoomClick()
	{
		PhotonNetwork.JoinRoom(roomNameText.text);
		createOrJoinRoom.roomListContent = this;
	}

	public void SetRoomInfo(RoomInfo roomInfo)
	{
		RoomInfo = roomInfo;
		roomNameText.text = roomInfo.Name.ToString();
		currentPlayersText.text = roomInfo.PlayerCount.ToString();
		maxPlayersText.text = roomInfo.MaxPlayers.ToString();
	}
}
