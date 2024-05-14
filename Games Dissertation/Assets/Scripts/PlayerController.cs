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
	private GameObject currentTileObject;
	private GameObject checkerObjectToMove;

	private GameObject tileObjectToMoveTo;

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

    void OnEnable()
    {
		playerActionMap["Press"].performed += OnPress;
		playerActionMap["Pan"].performed += OnPan;
		playerActionMap["Zoom"].performed += OnZoom;
	}

	void Update()
	{
		if (photonView != null)
		{
			if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
		}
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

		screenPosition = playerActionMap["ScreenPosition"].ReadValue<Vector2>();
		//Debug.Log($"Screen position: {screenPosition}");

		Ray ray = playerCamera.ScreenPointToRay(screenPosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.GetComponentInParent<Tile>() != null)
			{
				GameObject hitTile = hit.transform.GetComponentInParent<Tile>().gameObject;

				Debug.Log($"Raycast hit: {hitTile.transform.name}");

				if (hitTile != null && checkerObjectToMove == null && hitTile.GetComponentInChildren<Checker>())
				{
					currentTileObject = hitTile;
					checkerObjectToMove = currentTileObject.GetComponentInChildren<Checker>().gameObject;

					board.HighlightChecker(currentTileObject.GetComponent<Tile>());

					foreach (List<Tile> row in board.GetBoardList)
					{
						foreach (Tile tile in row)
						{
							if (board.IsValidMove(currentTileObject.GetComponent<Tile>(), tile))
							{
								board.HighlightBoardTile(tile);
							}
						}
					}
				}
				else if (hitTile == currentTileObject && checkerObjectToMove != null)
				{
					ResetVariables();
				}
				else if (hitTile != null && checkerObjectToMove != null)
				{
					tileObjectToMoveTo = hitTile;

					if (board.BoardTileIsHighlighted(tileObjectToMoveTo.GetComponent<Tile>()))
					{
						board.MoveCheckerWithTiles(currentTileObject.GetComponent<Tile>(), tileObjectToMoveTo.GetComponent<Tile>());
						board.HoppedChecker(currentTileObject.GetComponent<Tile>(), tileObjectToMoveTo.GetComponent<Tile>());

						ResetVariables();
					}
				}
			}
		}
	}

	private void ResetVariables()
	{
		currentTileObject = null;
		checkerObjectToMove = null;
		tileObjectToMoveTo = null;

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

	public void SetPlayerCheckerColor(Checker.CheckerColor checkerColor)
	{
		playerCheckerColor = checkerColor;
	}

	public Checker.CheckerColor GetPlayerCheckerColor()
	{
		return playerCheckerColor;
	}
}

