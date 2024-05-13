using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum GameScene
	{
		Loading,
		Lobby,
		Room,
		Game
	}

	public static GameManager Instance;
	private GameScene currentGameScene;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void ChangeGameScene(GameScene newGameScene)
	{
		currentGameScene = newGameScene;

		switch (currentGameScene)
		{
			case GameScene.Loading:
				SceneManager.LoadScene("Loading");
				break;

			case GameScene.Lobby:
				SceneManager.LoadScene("Lobby");
				break;

			case GameScene.Room:
				SceneManager.LoadScene("CharacterSelect");
				break;

			case GameScene.Game:
				SceneManager.LoadScene("Game");
				break;
		}
	}

	public GameScene GetGameScene()
	{
		return currentGameScene;
	}
}
