using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private Checker.CheckerColor? lastPlayerCheckerColor = null;

    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, this.transform.position, this.transform.rotation);
        SetPlayerCheckerColor(player.GetComponent<PlayerController>());
    }

    private void SetPlayerCheckerColor(PlayerController playerController)
    {
		if (lastPlayerCheckerColor == null)
        {
			playerController.SetPlayerCheckerColor(RandomCheckerColor());
            lastPlayerCheckerColor = playerController.GetPlayerCheckerColor();
		}
        else
        {
            playerController.SetPlayerCheckerColor((lastPlayerCheckerColor == Checker.CheckerColor.Black) ? Checker.CheckerColor.White : Checker.CheckerColor.Black);
            lastPlayerCheckerColor = playerController.GetPlayerCheckerColor();
        }
    }

    private Checker.CheckerColor RandomCheckerColor()
    {
        if (Random.value > 0.5f)
        {
            return Checker.CheckerColor.Black;
        }
        else
        {
            return Checker.CheckerColor.White;
        }
    }
}
