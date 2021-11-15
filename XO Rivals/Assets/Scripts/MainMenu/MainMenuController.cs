using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public GameObject MainMenuObject;
    [SerializeField] public GameObject PlayMenuObject;
    
    [SerializeField] public Button CreateGameButton;
    [SerializeField] public Button JoinGameButton;
    [SerializeField] public TMP_Text JoinOrBackButton_Text;
    
    public GameManager _gameManager;
    public PlayerInfo _localPlayer;

    private int Mode;

    #region UnityCB

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();
        Mode = 0;
    }

    #endregion

    #region MatchMethods

    public void ConnectRandomMatch()
    {
        _gameManager.OnConnectToRoom();
    }
    
    #endregion
    
    public void OnBackButtonClick()
    {
        switch (Mode)
        {
            case 0:
                MainMenuObject.SetActive(true);
                PlayMenuObject.SetActive(false);
                break;
            case 1:
                JoinOrBackButton_Text.text = "Back";
                GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Se canceló la búsqueda.";

                CreateGameButton.interactable = true;
                JoinGameButton.interactable = true;
            
                _gameManager.OnLeaveRoom();

                break;
            case 2:
                JoinOrBackButton_Text.text = "Back";

                CreateGameButton.interactable = true;
                JoinGameButton.interactable = true;
                break;
        }
    }

    public void ChangeMode(int n)
    {
        Mode = n;
    }
}
