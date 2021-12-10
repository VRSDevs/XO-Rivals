using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

/// <summary>
/// Tipos de salas a las que unirse
///     RANDOM_ROON - Sala aleatoria
///     SPECIFIC_ROOM - Sala específica
/// </summary>
public enum JoinType
{
    RANDOM_ROON,
    SPECIFIC_ROOM
}

public class NetworkController : MonoBehaviourPunCallbacks
{
    #region Variables
    
    ////////////////// CONEXIÓN //////////////////
    private bool _connected = false;
    
    ////////////////// SALAS //////////////////
    /// <summary>
    /// Jugadores mínimos en la sala
    /// </summary>
    private const int MIN_PLAYERS_IN_ROON = 2;
    /// <summary>
    /// Jugadores máximos en la sala
    /// </summary>
    private const int MAX_PLAYERS_INROOM = 2;
    /// <summary>
    /// ¿La partida está lista para comenzar?
    /// </summary>
    private bool _isReady = false;
    /// <summary>
    /// ¿Se está creando una partida?
    /// </summary>
    private bool _creatingRoom = false;
    /// <summary>
    /// Nombre de la sala específica
    /// </summary>
    private string _nameRoom = "";
    /// <summary>
    /// Tipo de unión a sala
    /// </summary>
    private JoinType _joinType;

    #endregion

    #region Getters

    /// <summary>
    /// Método para obtener si está conectado al servidor
    /// </summary>
    /// <returns>Valor de la conexión</returns>
    public bool IsConnected()
    {
        return _connected;
    }

    /// <summary>
    /// Método para obtener si la partida está lista para comenzar
    /// </summary>
    /// <returns>Estado de la partida</returns>
    private bool GetReadyStatus()
    {
        return _isReady;
    }

    /// <summary>
    /// Método para obtener si se está creando la partida o no
    /// </summary>
    /// <returns></returns>
    public bool GetCreatingRoom()
    {
        return _creatingRoom;
    }

    #endregion
    
    #region UnityCB

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #endregion
    
    #region ConnectionMethods

    /// <summary>
    /// Método para conectarse al servidor de Photon
    /// </summary>
    /// <returns>Valor del estado de la conexión</returns>
    public bool ConnectToServer()
    {
        return PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Método para desconectarse del servidor de Photon
    /// </summary>
    /// <returns></returns>
    public void DisconnectFromServer()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// Método de conexión a la lobby general
    /// </summary>
    public void ConnectToLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /// <summary>
    /// Método para conectarse a una sala activa.
    /// </summary>
    public void ConnectToRandomRoom()
    {
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Searching games...";
        StartCoroutine(JoinRoom());
    }

    /// <summary>
    /// Método para establecer conexión a una sala en específico
    /// </summary>
    /// <param name="name">ID de la sala</param>
    public void ConnectToSpecificRoom(string name)
    {
        _nameRoom = name;
        StartCoroutine(RejoinRoom());
    }

    /// <summary>
    /// Corutina para tratar de unirse a una sala
    /// </summary>
    /// <returns></returns>
    private IEnumerator JoinRoom()
    {
        yield return new WaitForSeconds(1);

        _joinType = JoinType.RANDOM_ROON;
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Corutina para tratar de unirse a una sala específica
    /// </summary>
    /// <returns></returns>
    private IEnumerator RejoinRoom()
    {
        yield return new WaitForSeconds(1);

        _joinType = JoinType.SPECIFIC_ROOM;
        PhotonNetwork.RejoinRoom(_nameRoom);
    }

    /// <summary>
    /// Método para abandonar la sala de la partida
    /// </summary>
    public void DisconnectFromRoom()
    {
        if (FindObjectOfType<GameManager>().IsPlaying)
        {
            if (FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].SurrenderStatus() ||
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].IsEnded())
            {
                FindObjectOfType<GameManager>().PlayerMatches.Remove(PhotonNetwork.CurrentRoom.Name);
                PhotonNetwork.LeaveRoom(false);
            }
        }
        
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region RoomsManagementMethods

    /// <summary>
    /// Método para crear una sala con un nombre en específico
    /// </summary>
    /// <param name="roomName">Nombre de la sala</param>
    private IEnumerator CreateMatchRoom(string roomName)
    {
        yield return new WaitForSeconds(1);
        
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions()
        {
            MaxPlayers = MAX_PLAYERS_INROOM,
            PlayerTtl = -1
        });
    }

    /// <summary>
    /// Corutina para empezar la partida que es ejecutada tras la preparación del jugador
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartMatch()
    {
        yield return new WaitUntil(GetReadyStatus);
        
        FindObjectOfType<GameManager>().IsPlaying = true;
        
        //Lose life and update server
        FindObjectOfType<MainMenuController>().ReduceLives();
        
        SceneManager.LoadScene("TicTacToe_Server");
    }
    
    #endregion

    #region PUN_CB

    /// <summary>
    /// CB cuando el usuario se conecta al servidor
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (SceneManager.GetActiveScene().name.Equals("Login"))
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Connected to server.";
        }

        _connected = true;
    }

    /// <summary>
    /// CB cuando el usuario se desconecta del servidor
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        _connected = false;

        switch (cause)
        {
            // Caso ClientTimeout -> El cliente deja de recibir respuestas por parte del servidor
            case DisconnectCause.ClientTimeout:
                FindObjectOfType<GameManager>().ResetObject();
                Destroy(GameObject.Find("PlayerObject"));

                SceneManager.LoadScene("Login");
                break;
            default:
                Debug.Log("Desconexión del servidor: " + cause);
                break;
        }
    }

    /// <summary>
    /// Método ejecutado cuando se conecta a la lobby
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        if (!SceneManager.GetActiveScene().name.Equals("Login")) return;
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Connected to the lobby.";
        Debug.Log("Hola, " + PhotonNetwork.NickName);

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// CB de Photon para cuando se crea la sala con éxito.
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Match created successfully.";
    }
    
    /// <summary>
    /// CB de Photon para cuando no se pudo crear la sala con éxito.
    /// </summary>
    /// <param name="returnCode">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        
        Debug.Log("Error " + returnCode + ": " + message);
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Couldn´t create the match.";
    }

    /// <summary>
    /// CB de Photon para cuando se une a la sala con éxito
    /// </summary>
    /// <param name="returnCode">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        FindObjectOfType<PlayerInfo>().MatchId = PhotonNetwork.LocalPlayer.ActorNumber;

        switch (_joinType)
        {
            case JoinType.RANDOM_ROON:
                
                UpdateCreatingStatus();

                if (PhotonNetwork.CurrentRoom.PlayerCount < MAX_PLAYERS_INROOM)
                {
                    GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Waiting for players (" + 
                        PhotonNetwork.CurrentRoom.Players.Count + "/" + MAX_PLAYERS_INROOM + ")...";
                }
                else
                {
                    FindObjectOfType<GameManager>().Matchmaking = false;
                    GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "¡Player found! Starting match...";
                    FindObjectOfType<GameManager>().SetupMatch("X");
                    StartCoroutine(StartMatch());
                }
                
                break;
            case JoinType.SPECIFIC_ROOM:
                FindObjectOfType<GameManager>().IsPlaying = true;
                SceneManager.LoadScene("TicTacToe_Server");
                break;
        }
    }

    /// <summary>
    /// CB de Photon ejecutado cuando falla el acceso a la sala aleatoria
    /// </summary>
    /// <param name="returnCode">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        switch (returnCode)
        {
            // Caso 32760 - Ninguna sala disponible
            case 32760:
                GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Couldn´t find any matches. Creating one...";

                StartCoroutine(CreateMatchRoom(GenerateRoomCode("Sala " + PhotonNetwork.CountOfRooms)));
                break;
            default:
                Debug.Log("Error " + returnCode + ": " + message);
                break;
        }
    }

    /// <summary>
    /// Método ejecutado cuando falla el acceso a la sala específica
    /// </summary>
    /// <param name="returnCode">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        
        Debug.Log("Error " + returnCode + ": " + message);
        
        StartCoroutine(CreateMatchRoom(_nameRoom));
    }

    /// <summary>
    /// CB de Photon para cuando se abandona la sala.
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        FindObjectOfType<PlayerInfo>().MatchId = -1;

        if (SceneManager.GetActiveScene().name.Equals("MainMenu") && !FindObjectOfType<GameManager>().IsPlaying)
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Matchmaking stopped.";
            UpdateCreatingStatus();
        }
        
        FindObjectOfType<GameManager>().IsPlaying = false;
    }

    /// <summary>
    /// Método ejecutado cuando se une un jugador a la sala
    /// </summary>
    /// <param name="newPlayer">Nuevo jugador unido</param>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
   
        if (PhotonNetwork.CurrentRoom.PlayerCount != MAX_PLAYERS_INROOM) return;

        FindObjectOfType<GameManager>().Matchmaking = false;
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "¡Player found! Starting match...";
        FindObjectOfType<GameManager>().SetupMatch("O");
        StartCoroutine(StartMatch());
    }

    /// <summary>
    /// Método ejecutado cuando un jugador abandona la sala
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        
        /*
        if (FindObjectOfType<GameManager>().IsPlaying)
            StartCoroutine(LeaveInMatch());
        */
    }

    #endregion

    #region UpdateNetworkVars

    /// <summary>
    /// Método para actualizar el nick en Photon
    /// </summary>
    /// <param name="nick"></param>
    public void SetNickName(string nick)
    {
        PhotonNetwork.NickName = nick;
    }
    
    /// <summary>
    /// Método para actualizar de manera automática el estado de preparación de la partida
    /// </summary>
    public void UpdateReadyStatus()
    {
        _isReady = !_isReady;
    }

    /// <summary>
    /// Método para actualizar de manera automática la variable de control de si está creando una partida o no
    /// </summary>
    public void UpdateCreatingStatus()
    {
        _creatingRoom = !_creatingRoom;
    }

    #endregion

    #region OtherMethods
    
    /// <summary>
    /// Método para generar una clave de sala
    /// </summary>
    /// <returns>Clave de sala</returns>
    private string GenerateRoomCode()
    {
        System.Random random = new System.Random();

    }

    #endregion
}
