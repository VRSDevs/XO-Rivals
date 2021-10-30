using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameManager _gameManager;
    public PlayerInfo _localPlayer;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _localPlayer = FindObjectOfType<PlayerInfo>();
    }

    public void OnCreateMatch()
    {
        _gameManager._networkController.OnCreateRoom();
        _gameManager.OnCreateRoom(_localPlayer);
    }

    public void OnConnectRandomMatch()
    {
        _gameManager._networkController.OnConnectToRandomRoom();
        _gameManager.OnConnectToRoom(_localPlayer);
    }
}
