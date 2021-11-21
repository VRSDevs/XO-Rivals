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

    [SerializeField] public TextMeshProUGUI nameTxt;
    [SerializeField] public TextMeshProUGUI level;
    [SerializeField] public TextMeshProUGUI lifesTxt;
    [SerializeField] public TextMeshProUGUI lifesTxtShop;
    [SerializeField] public TextMeshProUGUI lifesTime;
    [SerializeField] public Slider lvlSlider;
    
    ////////////////// CLASES //////////////////
    private GameManager _gameManager;
    private PlayerInfo _localPlayer;
    
    ////////////////// VIDAS //////////////////
    private DateTime recoverLifeTime;
    private TimeSpan recoverRemainingTime;
    private float timePassed = 0f;
    
    ////////////////// PARTIDA //////////////////
    private MatchInfo _matchToJoin;
    
    #endregion

    public AudioClip musica;
    #region UnityCB

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _localPlayer = GameObject.Find("PlayerObject").GetComponent<PlayerInfo>();

        JoinGameButton.interactable = false;

        nameTxt.text = _localPlayer.Name;
        level.text = "Level: " + Math.Truncate(_localPlayer.Level);
        lvlSlider.value = _localPlayer.Level % 1;
        lifesTxt.text = "Lives: " + _localPlayer.Lifes;
        
        _matchToJoin = new MatchInfo();

        if (_localPlayer.Lifes != 5){
            //recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(3);
            recoverLifeTime = _localPlayer.LostLifeTime.AddSeconds(15);
            CheckLifesTime();
        }else
            lifesTime.text = "-:--";
    }

    private void Update(){
        if(_localPlayer.Lifes != 5){
            timePassed += Time.deltaTime;
            if(timePassed >= 1.0f){ 
                CheckLifesTime();
                timePassed = 0f;
            }
        }
    }

    #endregion

    #region UpdateMethods

    private void IncreaseLifes(){

        _localPlayer.Lifes++;
        lifesTxt.text = "Lives: " + _localPlayer.Lifes;
        lifesTxtShop.text = "Lives: " + _localPlayer.Lifes;
        if (_localPlayer.Lifes < 5){
            _localPlayer.LostLifeTime = System.DateTime.Now;
            //Upload lifes to server
            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() {
                        {"Lifes", _localPlayer.Lifes.ToString()},
                        {"Life Lost", _localPlayer.LostLifeTime.ToString()}}
                },
                result => Debug.Log("Successfully updated user lifes"),
                error => {
                    Debug.Log("Got error setting user lifes");
                }
            );
            //Restart timer
            //recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(3);
            recoverLifeTime = _localPlayer.LostLifeTime.AddSeconds(10);
            recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
            lifesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds;            
        }else{
            //Upload lifes to server
            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() {
                        {"Lifes", _localPlayer.Lifes.ToString()},
                        {"Life Lost", ""}}
                },
                result => Debug.Log("Successfully updated user lifes"),
                error => {
                    Debug.Log("Got error setting user lifes");
                }
            );
            
            lifesTime.text = "-:--";
        }
    }

    private void ReduceLifes(){

        _localPlayer.Lifes--;
        lifesTxt.text = "Lives: " + _localPlayer.Lifes;
        lifesTxtShop.text = "Lives: " + _localPlayer.Lifes;
        //If it has 4 lifes, update timer
        if(_localPlayer.Lifes == 4){
            _localPlayer.LostLifeTime = System.DateTime.Now;
            //Upload lifes to server
            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() {
                        {"Lifes", _localPlayer.Lifes.ToString()},
                        {"Life Lost", _localPlayer.LostLifeTime.ToString()}}
                },
                result => Debug.Log("Successfully reduced user lifes"),
                error => {
                    Debug.Log("Got error reducing user lifes");
                }
            );
            //Restart timer
            //recoverLifeTime = _localPlayer.LostLifeTime.AddMinutes(3);
            recoverLifeTime = _localPlayer.LostLifeTime.AddSeconds(10);
            recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
            lifesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds;            
        }else{
            //Upload lifes to server
            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() {
                        {"Lifes", _localPlayer.Lifes.ToString()}}
                },
                result => Debug.Log("Successfully reduced user lifes"),
                error => {
                    Debug.Log("Got error reducing user lifes");
                }
            );
            
            lifesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds;
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
        _gameManager.Matchmaking = !_gameManager.Matchmaking;
        
        CreateGameButton.onClick.RemoveAllListeners();

        CreateGameButton.interactable = false;
        JoinGameButton.interactable = false;
        ChangeMatchListInteractions(false);
        BackButton.interactable = false;

        if (_gameManager.Matchmaking)
        {
            StartCoroutine(ChangeInteractionAfterCm("connect"));
            //Lose life and update server
            ReduceLifes();
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
                child.GetComponent<Button>().interactable = false;
                JoinGameButton.interactable = true;
                _matchToJoin.MatchId = selectedChildren["MatchID"].GetComponent<TextMeshProUGUI>().text;
                _matchToJoin.MatchName = selectedChildren["MatchName"].GetComponent<TextMeshProUGUI>().text;
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
        _gameManager.OnDisconnectToServer();

        _gameManager.ResetObject();
        Destroy(GameObject.Find("PlayerObject"));

        SceneManager.LoadScene("Login");
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
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Joined the match.";
        
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

    private void CheckLifesTime(){

        recoverRemainingTime = recoverLifeTime.Subtract(System.DateTime.Now);
        //Check remainingTime
        if(recoverRemainingTime < TimeSpan.Zero){
            //Recover one life
            IncreaseLifes();
        }else{
            lifesTime.text = "" + recoverRemainingTime.Minutes + ":" + recoverRemainingTime.Seconds;  
        }
    }

    #endregion
}
