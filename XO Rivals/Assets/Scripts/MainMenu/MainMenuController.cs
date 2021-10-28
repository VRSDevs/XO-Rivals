using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void OnConnectToRandomMatch()
    {
        _gameManager._networkController.OnCreateRoom();
    }

    public void goToGame(){
        SceneManager.LoadScene("Tic Tac Toe");
    }

    public void goToSettings(){
        SceneManager.LoadScene("Tic Tac Toe");
    }

    public void goToCredits(){
        SceneManager.LoadScene("Tic Tac Toe");
    }
}
