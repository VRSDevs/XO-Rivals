using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Serialization;

public struct MatchInfo
{
    public string MatchId { get; set; }
    public string MatchName { get; set; }

    public MatchInfo(string id, string name)
    {
        MatchId = id;
        MatchName = name;
    }
}

public class MainMenuController : MonoBehaviour
{
    #region Vars

    ////////////////// REFERENCIAS //////////////////
    [SerializeField] public GameObject MainMenuObject;
    [SerializeField] public GameObject PlayMenuObject;
    
    [SerializeField] public Button CreateGameButton;
    [SerializeField] public Button JoinGameButton;
    [SerializeField] public Button BackButton;

    [SerializeField] public Sprite CreateMatchSprite;
    [SerializeField] public Sprite CancelMatchmakingSprite;

    [SerializeField] public Image CreateMatchImage;

    [SerializeField] public GameObject ViewContent;

    [SerializeField] public TextMeshProUGUI NameTxt;
    [SerializeField] public TextMeshProUGUI LevelTxt;
    [SerializeField] public TextMeshProUGUI LivesTxt;
    [SerializeField] public TextMeshProUGUI LivesTxtShop;
    [SerializeField] public TextMeshProUGUI LivesTime;
    [SerializeField] public TextMeshProUGUI LvlPrcntg;
    [SerializeField] public Slider LvlSlider;
    
    ////////////////// CLASES //////////////////
    private GameManager _gameManager;
    private PlayerInfo _localPlayer;
    
    ////////////////// VIDAS //////////////////
    private DateTime recoverLifeTime;
    private TimeSpan recoverRemainingTime;
    private float timePassed = 0f;
    private const int MAXLIVES = 3;
    
    ////////////////// PARTIDA //////////////////
    private MatchInfo _matchToJoin;
    
    ////////////////// MÚSICA //////////////////
    public AudioClip MusicObject;
    public AudioClip Main_menu;
    public AudioClip Tic_Tac_toe_Music;
    #endregion
    
    #region UnityCB

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

        FindObjectOfType<AudioManager>().ChangeMusic(Main_menu,"Tic-Tac-Toe");
        //FindObjectOfType<AudioManager>().Play("Main_menu");
        FindObjectOfType<AudioManager>().Stop("Main_menu");
        FindObjectOfType<AudioManager>().Play("Main_Menu");

        //JoinGameButton.interactable = false;

        NameTxt.text = _localPlayer.Name;
        LevelTxt.text = "Level: " + Math.Truncate(_localPlayer.Level);
        LvlSlider.value = _localPlayer.Level % 1;
        LvlPrcntg.text = (int)((_localPlayer.Level % 1) * 100) + "/100"; 
        LivesTxt.text = "Lives: " + _localPlayer.Lives;
        LivesTxtShop.text = "Lives: " + _localPlayer.Lives;
        
        _matchToJoin = new MatchInfo();

        if (_localPlayer.Lives < MAXLIVES){
            recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(30);
            CheckLivesTime();
        }else
            LivesTime.text = "Lives container is full";
    }

    private void Update(){

        if(_localPlayer.Lives < MAXLIVES){
            timePassed += Time.deltaTime;
            if(timePassed >= 1.0f){ 
                CheckLivesTime();
                timePassed = 0f;
            }
        }
    }

    #endregion

    #region UpdateMethods

    /// <summary>
    /// Método para aumentar las vidas
    /// </summary>
    private void IncreaseLives(){
        
        _localPlayer.Lives++;
        LivesTxt.text = "Lives: " + _localPlayer.Lives;
        LivesTxtShop.text = "Lives: " + _localPlayer.Lives;

        if (_localPlayer.Lives < MAXLIVES)
        {
            _localPlayer.LostLifeTime = System.DateTime.Now;
            
            _gameManager.UpdateCloudData(new Dictionary<string, string>() {
                {DataType.Lives.GetString(), _localPlayer.Lives.ToString()},
                {DataType.LifeLost.GetString(), _localPlayer.LostLifeTime.ToString("dd/MM/yyyy HH:mm:ss")}
            },
                DataType.Lives);
            
            //Restart timer
            recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(30);
            recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
            LivesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds + " min"; 
        } else
        {
            _gameManager.UpdateCloudData(new Dictionary<string, string>() {
                {DataType.Lives.GetString(), _localPlayer.Lives.ToString()},
                {DataType.LifeLost.GetString(), ""}
            },
                DataType.Lives);
            
            LivesTime.text = "Lives container is full";
        }
    }

    /// <summary>
    /// Método para reducir el número de vidas
    /// </summary>
    public void ReduceLives()
    {
        //Reduce only if it doesnt have infinite (999) lives
        if (_localPlayer.Lives == 999) return;
        
        _localPlayer.Lives--;
        LivesTxt.text = "Lives: " + _localPlayer.Lives;
        LivesTxtShop.text = "Lives: " + _localPlayer.Lives;
            
        //If it has MAXLIVES - 1 lives, update timer
        if (_localPlayer.Lives == MAXLIVES - 1){
            
            _localPlayer.LostLifeTime = System.DateTime.Now;
            
            //Upload lives to server
            _gameManager.UpdateCloudData(new Dictionary<string, string>() {
                {DataType.Lives.GetString(), _localPlayer.Lives.ToString()},
                {DataType.LifeLost.GetString(), _localPlayer.LostLifeTime.ToString("dd/MM/yyyy HH:mm:ss")}
            },
                DataType.Lives);
            
            //Restart timer
            recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(30);
            recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
            LivesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds; 
            
        } else
        {
            //Upload lives to server
            _gameManager.UpdateCloudData(new Dictionary<string, string>() {
                {DataType.Lives.GetString(), _localPlayer.Lives.ToString()},
            },
                DataType.Lives);
                
            LivesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds + " min";
        }
    }

    #endregion

    #region MatchMethods

    /// <summary>
    /// Método para conectarse a una sala aleatoria
    /// </summary>
    private void ConnectRandomMatch()
    {
        _gameManager.OnConnectToRoom();
    }

    /// <summary>
    /// Método para conectarse a una sala en específico
    /// </summary>
    private void ConnectToMatch()
    {
        _gameManager.OnConnectToSpecificRoom(_matchToJoin.MatchId);
    }

    /// <summary>
    /// Método para abandonar la búsqueda de partidas
    /// </summary>
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
        CreateGameButton.onClick.RemoveAllListeners();

        CreateGameButton.interactable = false;
        JoinGameButton.interactable = false;
        ChangeMatchListInteractions(false);
        BackButton.interactable = false;

        if (_gameManager.Matchmaking)
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Searching games...";
            StartCoroutine(CreateOrCancelPMatch("connect"));
        }
        else
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Stopping matchmaking...";
            StartCoroutine(CreateOrCancelPMatch("cancel"));
        }
    }

    /// <summary>
    /// Método de evento ejecutado al pulsar el botón de unirse a partida
    /// </summary>
    private void OnJoinMatchClick()
    {
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Joining " + _matchToJoin.MatchName + "...";
        
        StartCoroutine(ChangeInteractionAfterJm());
        
        CreateGameButton.interactable = false;
        JoinGameButton.interactable = false;
        
        ConnectToMatch();
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
                _matchToJoin.MatchId = selectedChildren["MatchID"].GetComponent<TextMeshProUGUI>().text;
                _matchToJoin.MatchName = selectedChildren["MatchName"].GetComponent<TextMeshProUGUI>().text;
                OnJoinMatchClick();
                continue;
            }
            
            child.GetComponent<Button>().interactable = false;
        }
    }
    
    /// <summary>
    /// Método de evento ejecutado cuando se pulsa el botón de salir
    /// </summary>
    public void OnExitClick()
    {
        _gameManager.OnDisconnectToServer();
    }

    #endregion

    #region ButtonInteractionsMethods

    /// <summary>
    /// Corutina ejecutada tras crear una partida o buscarla
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateOrCancelPMatch(string mode)
    {
        yield return new WaitForSeconds(2);
        
        switch (mode)
        {
            case "connect":
                if (_localPlayer.Lives < 1)
                {
                    GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "You can´t look for a match." +
                        " You don´t have enough lives.";
                    
                    CreateGameButton.interactable = true;
                    JoinGameButton.interactable = true;
                    ChangeMatchListInteractions(true);
                    BackButton.interactable = true;
                    
                }
                else
                {
                    _gameManager.Matchmaking = !_gameManager.Matchmaking;
                    ConnectRandomMatch();
                    CreateMatchImage.sprite = CancelMatchmakingSprite;
                }
                
                break;
            case "cancel":
                _gameManager.Matchmaking = !_gameManager.Matchmaking;
                
                LeaveMatchmaking();
                CreateMatchImage.sprite = CreateMatchSprite;
                
                ChangeMatchListInteractions(true);
                BackButton.interactable = true;
                
                break;
        }
    }
    
    /// <summary>
    /// Método de corutina ejecutado para resetear los botones del menú de jugar
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeInteractionAfterJm()
    {
        yield return new WaitForSeconds(2);

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

    #region OtherMethods

    public void SelectButton1()
    { 
        FindObjectOfType<AudioManager>().Play("SelecctionButton1");
    }    
    
    private void CheckLivesTime(){
        recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
        //Check remainingTime
        if(recoverRemainingTime < TimeSpan.Zero){
            //Recover one life
            IncreaseLives();
        }else{
            LivesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds + " min";  
        }
    }

    #endregion
}
