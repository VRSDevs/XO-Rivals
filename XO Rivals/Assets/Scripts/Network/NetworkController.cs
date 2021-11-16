using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using Scene = UnityEditor.SearchService.Scene;

public class NetworkController : MonoBehaviourPunCallbacks
{
    #region Variables

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
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Buscando salas...";
        StartCoroutine(JoinRoom());
    }

    /// <summary>
    /// Corutina para tratar de unirse a una sala
    /// </summary>
    /// <returns></returns>
    private IEnumerator JoinRoom()
    {
        yield return new WaitForSeconds(1);
        
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Método para abandonar la sala de la partida
    /// </summary>
    public void DisconnectFromRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        
        if (!FindObjectOfType<GameManager>().IsPlaying) return;
        FindObjectOfType<GameManager>().MatchId = "";
        FindObjectOfType<GameManager>().OwnerId = "";
        FindObjectOfType<GameManager>().PlayerInfoO = null;
        FindObjectOfType<GameManager>().PlayerInfoX = null;
        FindObjectOfType<GameManager>().WhosTurn = "";
        FindObjectOfType<GameManager>().NumFilled = 0;
        FindObjectOfType<GameManager>().FilledPositions = new int[3,3];
        FindObjectOfType<GameManager>().turnMoment = 0;
        FindObjectOfType<GameManager>().Chips = new List<GameObject>();
        FindObjectOfType<GameManager>().MiniGameChosen = 0;
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
        });
    }

    private IEnumerator StartMatch()
    {
        yield return new WaitUntil(GetReadyStatus);
        
        FindObjectOfType<GameManager>().IsPlaying = true;
        SceneManager.LoadScene("TicTacToe_Server");
    }
    
    #endregion

    #region PUN_CB

    /// <summary>
    /// Método ejecutado cuando se conecta al servidor
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        if (SceneManager.GetActiveScene().name.Equals("Login"))
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Conectado al servidor. Conectando al lobby...";
        }
        
        ConnectToLobby();
    }

    /// <summary>
    /// Método ejecutado cuando se conecta a la lobby
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        if (!SceneManager.GetActiveScene().name.Equals("Login")) return;
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Conectado al lobby general.";
        Debug.Log("Hola, " + PhotonNetwork.NickName);

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// CB de Photon para cuando se crea la sala con éxito.
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Sala creada con éxito.";
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
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Fallo al crear la sala.";
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        FindObjectOfType<PlayerInfo>().MatchId = PhotonNetwork.LocalPlayer.ActorNumber;

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Buscando jugadores...";
        }
        else
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "¡Jugador encontrado! Empezando partida...";
            FindObjectOfType<GameManager>().SetupMatch("X");
            StartCoroutine(StartMatch());
        }
    }

    /// <summary>
    /// Método ejecutado cuando falla el acceso a la sala aleatoria
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
                GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "No hay salas activas. Creando una...";

                StartCoroutine(CreateMatchRoom(GetHashValue("Sala " + PhotonNetwork.CountOfRooms)));
                break;
            default:
                Debug.Log("Error " + returnCode + ": " + message);
                break;
        }
    }

    /// <summary>
    /// CB de Photon para cuando se abandona la sala.
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Búsqueda cancelada.";
        }
    }

    /// <summary>
    /// Método ejecutado cuando se une un jugador a la sala
    /// </summary>
    /// <param name="newPlayer">Nuevo jugador unido</param>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount != MAX_PLAYERS_INROOM) return;

        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "¡Jugador encontrado! Empezando partida...";
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

        if (FindObjectOfType<GameManager>().IsPlaying)
        {
            FindObjectOfType<EndGameScript>().ShowSurrenderVictory();
            FindObjectOfType<GameManager>().IsPlaying = false;
        }
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

    #region OtherMethods

    /// <summary>
    /// Método para obtener si la partida está lista para comenzar
    /// </summary>
    /// <returns>Estado de la partida</returns>
    private bool GetReadyStatus()
    {
        return _isReady;
    }

    /// <summary>
    /// Método para transformar un dato en un valor hash
    /// </summary>
    /// <param name="data">Dato a transformar</param>
    /// <returns>Valor hash del dato introducido</returns>
    private string GetHashValue(string data)
    {
        var hash = new Hash128();
        hash.Append(data);
        
        return hash.ToString();
    }

    #endregion
}
