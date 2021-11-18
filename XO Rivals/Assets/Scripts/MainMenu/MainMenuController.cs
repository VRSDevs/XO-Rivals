using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] public GameObject MainMenuObject;
    [SerializeField] public GameObject PlayMenuObject;
    
    [SerializeField] public Button CreateGameButton;
    [SerializeField] public Button JoinGameButton;

    [SerializeField] public Sprite CreateMatchSprite;
    [SerializeField] public Sprite CancelMatchmakingSprite;

    [SerializeField] public GameObject ViewContent;

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

    public void OnMatchModelClick()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        
        Dictionary<string, GameObject> selectedChildren = new Dictionary<string, GameObject>();

        for (int i = 0; i < selectedButton.transform.childCount; i++)
        {
            selectedChildren.Add(selectedButton.transform.GetChild(i).gameObject.name, selectedButton.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ViewContent.transform.childCount; i++)
        {
            GameObject child = ViewContent.transform.GetChild(i).gameObject;

            Dictionary<string, GameObject> grandChildren = new Dictionary<string, GameObject>();

            for (int j = 0; j < child.transform.childCount; j++)
            {
                Debug.Log(child.transform.GetChild(j).gameObject.name);
                grandChildren.Add(child.transform.GetChild(j).gameObject.name, child.transform.GetChild(j).gameObject);
            }

            if (grandChildren["MatchName"].GetComponent<TextMeshProUGUI>().text.Equals(selectedChildren["MatchName"].GetComponent<TextMeshProUGUI>().text))
            {
                Debug.Log("Igual");
                child.GetComponent<Button>().interactable = false;
                JoinGameButton.interactable = true;
                break;
            }
            
            child.GetComponent<Button>().interactable = true;
        }
        
        Debug.Log("Pulsado: " + selectedButton.name);
    }
    
    /// <summary>
    /// Método de evento ejecutado cuando se pulsa el botón de salir
    /// </summary>
    public void OnExitClick()
    {
        Application.ExternalEval("document.location.reload(true)");
    }

    #endregion

    public void ChangeMode(int n)
    {
        Mode = n;
    }
}
