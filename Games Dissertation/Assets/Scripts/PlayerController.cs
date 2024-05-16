using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	// Photon
	[HideInInspector]
	public PhotonView photonView;

	// Input actions
	private PlayerInput playerInput;
	private InputActionAsset inputActionAsset;
	private InputActionMap playerActionMap;

	private Vector2 screenPosition;

	private Board board;

	// Moving checkers
	private Tile currentTile;
	private GameObject checkerObjectToMove;

	private Tile tileToMoveTo;

	[Header("Camera Settings")]
	[SerializeField]
	private GameObject playerCameraController;
	[SerializeField]
	private Camera playerCamera;

	[SerializeField]
	private float panSpeed = 5f;

	[SerializeField]
	private float zoomSpeed = 5f;

	private float xAngle;
	private float yAngle;
	private float zAngle;

	[Header("Audio Listener")]
	[SerializeField]
	private AudioListener playerAudioListener;

	// Player checker color
	[SerializeField]
	private Checker.CheckerColor playerCheckerColor;

	private GameObject whiteOrBlackCanvas;
	private GameObject waitingForRoomOwnerCanvas;
	private GameObject checkersWinCanvas;

	void Awake()
    {
		// Photon
		photonView = this.GetComponent<PhotonView>();

		// Input actions
		playerInput = this.GetComponent<PlayerInput>();

		inputActionAsset = playerInput.actions;
		playerActionMap = inputActionAsset.FindActionMap("Player");

		inputActionAsset.Enable();

		board = FindObjectOfType<Board>();

		// Camera settings
		xAngle = playerCamera.transform.rotation.eulerAngles.x;
		yAngle = playerCamera.transform.rotation.eulerAngles.y;
		zAngle = playerCamera.transform.rotation.eulerAngles.z;

		playerCamera.transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);

		whiteOrBlackCanvas = FindAnyObjectByType<PlayerSpawner>().GetWhiteOrBlackCanvas;
		waitingForRoomOwnerCanvas = FindAnyObjectByType<PlayerSpawner>().GetWaitingForRoomOwnerCanvas;
		checkersWinCanvas = board.GetCheckersWinCanvas;
	}

	void Start()
	{
		if (photonView != null)
		{
			if (photonView.IsMine)
			{
				playerCameraController.SetActive(true);
				playerAudioListener.enabled = true;
			}
			else
			{
				playerCameraController.SetActive(false);
				playerAudioListener.enabled = false;
			}
		}
	}

	void Update()
	{
		if (photonView != null)
		{
			if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

			if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomOwnerCheckerColor", out object roomOwnerCheckerColor))
			{
				if (PhotonNetwork.IsMasterClient)
				{
					photonView.RPC("SetPlayerCheckerColor", RpcTarget.AllBuffered, ((Checker.CheckerColor)roomOwnerCheckerColor));
					SetCameraRotation();
				}
				else
				{
					photonView.RPC("SetPlayerCheckerColor", RpcTarget.AllBuffered, ((Checker.CheckerColor)roomOwnerCheckerColor ==
						Checker.CheckerColor.Black) ? Checker.CheckerColor.White : Checker.CheckerColor.Black);
					SetCameraRotation();
				}
			}
		}
	}

    void OnEnable()
    {
		playerActionMap["Press"].performed += OnPress;
		playerActionMap["Pan"].performed += OnPan;
		playerActionMap["Zoom"].performed += OnZoom;
	}

    void OnDisable()
    {
		playerActionMap["Press"].performed -= OnPress;
		playerActionMap["Pan"].performed -= OnPan;
		playerActionMap["Zoom"].performed -= OnZoom;
	}

	private void OnPress(InputAction.CallbackContext context)
    {
		if (!photonView.IsMine) return;
		if (whiteOrBlackCanvas.activeInHierarchy || waitingForRoomOwnerCanvas.activeInHierarchy 
			|| checkersWinCanvas.activeInHierarchy) return;

		screenPosition = playerActionMap["ScreenPosition"].ReadValue<Vector2>();
		//Debug.Log($"Screen position: {screenPosition}");

		Ray ray = playerCamera.ScreenPointToRay(screenPosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.GetComponentInParent<Tile>() != null)
			{
				Tile hitTile = hit.transform.GetComponentInParent<Tile>();
				Debug.Log($"Raycast hit: {hitTile.transform.name}, {board.FindTilePosition(hitTile)}");

				if (hitTile != null && checkerObjectToMove == null && hitTile.GetComponentInChildren<Checker>())
				{
					FindValidMovesForChecker(hitTile);
				}
				else if (hitTile == currentTile && checkerObjectToMove != null)
				{
					ResetVariables();
				}
				else if (hitTile != null && checkerObjectToMove != null)
				{
					MoveCheckerIfTileHighlighted(hitTile);
				}
			}
		}
	}

	private void FindValidMovesForChecker(Tile hitTile)
	{
		if (hitTile.GetComponentInChildren<Checker>().GetCheckerColor == playerCheckerColor)
		{
			currentTile = hitTile;
			checkerObjectToMove = currentTile.GetComponentInChildren<Checker>().gameObject;

			board.HighlightChecker(currentTile);

			foreach (List<Tile> row in board.GetBoardList)
			{
				foreach (Tile tile in row)
				{
					if (board.IsValidMove(currentTile, tile))
					{
						board.HighlightBoardTile(tile);
					}
				}
			}
		}
	}

	private void MoveCheckerIfTileHighlighted(Tile hitTile)
	{
		tileToMoveTo = hitTile;

		if (board.BoardTileIsHighlighted(tileToMoveTo))
		{
			board.MoveCheckerWithTiles(currentTile, tileToMoveTo);
			board.HoppedChecker(currentTile, tileToMoveTo);

			if (board.CheckerReachedOtherSideOfBoard(currentTile, tileToMoveTo))
			{
				if (!currentTile.GetComponentInChildren<Checker>().King)
				{
					board.MakeCheckerKing(currentTile, tileToMoveTo);
					ResetVariables();
				}
				else
				{
					ResetVariables();
				}
			}
			else
			{
				ResetVariables();
			}
		}
	}

	private void ResetVariables()
	{
		currentTile = null;
		checkerObjectToMove = null;
		tileToMoveTo = null;

		board.RemoveHighlightFromBoardTiles();
		board.RemoveHighlightFromChecker();
	}

	private void OnPan(InputAction.CallbackContext	context)
	{
		if (!photonView.IsMine) return;

		Vector2 delta = context.ReadValue<Vector2>();

		Vector3 movement = new Vector3(delta.y, 0, -delta.x) * panSpeed * Time.deltaTime;  // Pan affects the x and z axis
		playerCameraController.transform.Translate(movement, Space.World);
	}

	private void OnZoom(InputAction.CallbackContext context)
	{
		if (!photonView.IsMine) return;

		float zoom = context.ReadValue<Vector2>().y;

		// Set rotation of camera
		Quaternion rotation = Quaternion.Euler(xAngle, yAngle, zAngle);

		// Calculate the forward direction based on the rotation
		Vector3 calculatedForward = rotation * Vector3.forward;

		// Zoom happens along the calculated forward direction
		Vector3 movement = zoom * zoomSpeed * Time.deltaTime * calculatedForward;
		playerCameraController.transform.Translate(movement, Space.Self);
	}

	[PunRPC]
	public void SetPlayerCheckerColor(Checker.CheckerColor checkerColor)
	{
		playerCheckerColor = checkerColor;
	}

	public Checker.CheckerColor GetPlayerCheckerColor()
	{
		return playerCheckerColor;
	}

	private void SetCameraRotation()
	{
		if (playerCheckerColor == Checker.CheckerColor.Black)
		{
			playerCameraController.transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		else
		{
			playerCameraController.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}

