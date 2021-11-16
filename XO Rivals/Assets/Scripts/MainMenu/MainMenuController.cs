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
    
    /*
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
            
                FindObjectOfType<GameManager>().OnLeaveRoom();

                break;
            case 2:
                JoinOrBackButton_Text.text = "Back";

                CreateGameButton.interactable = true;
                JoinGameButton.interactable = true;
                break;
        }
    }
    */

    #region ButtonsMethods

    /// <summary>
    /// Método para actualizar el comportamiento del botón de crear partida
    /// </summary>
    public void OnCreateMatchClick()
    {
        _gameManager.Matchmaking = !_gameManager.Matchmaking;
        
        Debug.Log("¿Buscando partida? " + _gameManager.Matchmaking);
        
        CreateGameButton.onClick.RemoveAllListeners();
        
        if (_gameManager.Matchmaking)
        {
            CreateGameButton.onClick.AddListener(ConnectRandomMatch);
            CreateGameButton.GetComponent<Image>().sprite = CancelMatchmakingSprite;
            CreateGameButton.onClick.RemoveListener(LeaveMatchmaking);
        }
        else
        {
            CreateGameButton.onClick.AddListener(LeaveMatchmaking);
            CreateGameButton.GetComponent<Image>().sprite = CreateMatchSprite;
            CreateGameButton.onClick.RemoveListener(ConnectRandomMatch);
        }
    }

    #endregion

    public void ChangeMode(int n)
    {
        Mode = n;
    }
}
