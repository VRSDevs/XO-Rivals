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

    [SerializeField] public Sprite CreateMatchSprite;
    [SerializeField] public Sprite CancelMatchmakingSprite;

    public RectTransform MatchPrefab;
    public RectTransform ViewContent;

    private GameManager _gameManager;
    private PlayerInfo _localPlayer;

    private int Mode;

    #region UnityCB

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

        JoinGameButton.interactable = false;
        
        Mode = 0;
    }

    #endregion

    #region UpdateMethods

    public void UpdateMatches()
    {
        FetchPlayerMatches(5, matchesList =>
        {
            OnRecievedMatches(matchesList);
        });
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

    private void FetchPlayerMatches(int count, Action<MatchModel[]> onDone)
    {
        var matchesList = new MatchModel[count];
        for (int i = 0; i < matchesList.Length; i++)
        {
            matchesList[i] = new MatchModel();
            matchesList[i].MatchName = "Sala " + i;
            matchesList[i].MatchStatus = "Turno de X";
        }

        onDone(matchesList);
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
    
    /// <summary>
    /// Método de evento ejecutado cuando se pulsa el botón de salir
    /// </summary>
    public void OnExitClick()
    {
        Application.ExternalEval("document.location.reload(true)");
    }

    #endregion

    #region OtherMethods

    private void OnRecievedMatches(MatchModel[] list)
    {
        foreach (Transform child in ViewContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var matchModel in list)
        {
            var instance = GameObject.Instantiate(MatchPrefab.gameObject, ViewContent, false);
        }
    }

    #endregion

    public void ChangeMode(int n)
    {
        Mode = n;
    }
}
