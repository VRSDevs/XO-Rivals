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
    [SerializeField] public Button BackButton;

    [SerializeField] public Sprite CreateMatchSprite;
    [SerializeField] public Sprite CancelMatchmakingSprite;

    [SerializeField] public Image CreateMatchImage;

    [SerializeField] public GameObject ViewContent;

    private GameManager _gameManager;
    private PlayerInfo _localPlayer;

    private string MatchName;

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
    /// Método para mostrar el menú de jugar
    /// </summary>
    public void OnPlayMenuClick()
    {
        MainMenuObject.SetActive(false);
        PlayMenuObject.SetActive(true);
    }

    /// <summary>
    /// Método para actualizar el comportamiento del botón de crear partida
    /// </summary>
    public void OnCreateMatchClick()
    {
        _gameManager.Matchmaking = !_gameManager.Matchmaking;
        
        CreateGameButton.onClick.RemoveAllListeners();

        CreateGameButton.interactable = false;
        JoinGameButton.interactable = false;
        ChangeMatchListInteractions(false);
        BackButton.interactable = false;

        if (_gameManager.Matchmaking)
        {
            StartCoroutine(ChangeInteractionAfterCm("connect"));
            ConnectRandomMatch();
            CreateMatchImage.sprite = CancelMatchmakingSprite;
        }
        else
        {
            StartCoroutine(ChangeInteractionAfterCm("cancel"));
            LeaveMatchmaking();
            CreateMatchImage.sprite = CreateMatchSprite;
        }
    }

    /// <summary>
    /// Método de evento ejecutado al pulsar el botón de unirse a partida
    /// </summary>
    public void OnJoinMatchClick()
    {
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Uniéndote a " + MatchName + " (BETA)";
        CreateGameButton.interactable = false;
        JoinGameButton.interactable = false;
        StartCoroutine(ChangeInteractionAfterJm());
    }

    /// <summary>
    /// Método de evento ejecutado cuando se pulsa una partida de la lista de partidas
    /// </summary>
    public void OnMatchModelClick()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        
        Dictionary<string, GameObject> selectedChildren = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> grandChildren = new Dictionary<string, GameObject>();
        
        for (int i = 0; i < selectedButton.transform.childCount; i++)
        {
            selectedChildren.Add(selectedButton.transform.GetChild(i).gameObject.name, selectedButton.transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < ViewContent.transform.childCount; i++)
        {
            GameObject child = ViewContent.transform.GetChild(i).gameObject;

            grandChildren.Clear();
            
            for (int j = 0; j < child.transform.childCount; j++)
            {
                grandChildren.Add(child.transform.GetChild(j).gameObject.name, child.transform.GetChild(j).gameObject);
            }

            if (grandChildren["MatchName"].GetComponent<TextMeshProUGUI>().text.Equals(selectedChildren["MatchName"].GetComponent<TextMeshProUGUI>().text))
            {
                child.GetComponent<Button>().interactable = false;
                JoinGameButton.interactable = true;
                MatchName = selectedChildren["MatchName"].GetComponent<TextMeshProUGUI>().text;
                continue;
            }
            
            child.GetComponent<Button>().interactable = true;
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

    #region ButtonInteractionsMethods

    /// <summary>
    /// Corutina ejecutada tras crear una partida o buscarla
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeInteractionAfterCm(string mode)
    {
        yield return new WaitUntil(_gameManager.GetIsCreatingMatch);

        CreateGameButton.interactable = true;

        if (mode.Equals("cancel"))
        {
            JoinGameButton.interactable = true;
            ChangeMatchListInteractions(true);
            BackButton.interactable = true;
        }
        
        _gameManager.SetCreatingRoomStatus();
    }
    
    /// <summary>
    /// Método de corutina ejecutado para resetear los botones del menú de jugar
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeInteractionAfterJm()
    {
        yield return new WaitForSeconds(2);
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Te hubieses unido a la sala";
        
        CreateGameButton.interactable = true;

        ChangeMatchListInteractions(true);
    }

    /// <summary>
    /// Método para cambiar la interacción con las partidas de la lista de partidas del jugador
    /// </summary>
    /// <param name="interactable"></param>
    private void ChangeMatchListInteractions(bool interactable)
    {
        for (int i = 0; i < ViewContent.transform.childCount; i++)
        {
            GameObject child = ViewContent.transform.GetChild(i).gameObject;

            child.GetComponent<Button>().interactable = interactable;
        }
    }

    #endregion
}
