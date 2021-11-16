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

    [SerializeField] public Sprite CreateMatchSprite;
    [SerializeField] public Sprite CancelMatchmakingSprite;

    private GameManager _gameManager;
    private PlayerInfo _localPlayer;

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

    private void ConnectRandomMatch()
    {
        _gameManager.OnConnectToRoom();
    }

    private void LeaveMatchmaking()
    {
        _gameManager.OnLeaveRoom();
    }
    
    #endregion

    #region ButtonsMethods

    /// <summary>
    /// Método para actualizar el comportamiento del botón de crear partida
    /// </summary>
    public void OnCreateMatchClick()
    {
        _gameManager.Matchmaking = !_gameManager.Matchmaking;

        CreateGameButton.onClick.RemoveAllListeners();
        
        if (_gameManager.Matchmaking)
        {
            ConnectRandomMatch();
            CreateGameButton.GetComponent<Image>().sprite = CancelMatchmakingSprite;
        }
        else
        {
            LeaveMatchmaking();
            CreateGameButton.GetComponent<Image>().sprite = CreateMatchSprite;
        }
    }

    #endregion

    public void ChangeMode(int n)
    {
        Mode = n;
    }
}
