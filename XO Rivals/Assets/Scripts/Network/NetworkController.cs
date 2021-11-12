using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

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

    #endregion
    
    #region ConnectMethods

    /// <summary>
    /// Método para conectarse al servidor de Photon
    /// </summary>
    /// <returns>Valor del estado de la conexión</returns>
    public bool OnConnectToServer()
    {
        return PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Método de conexión a la lobby general
    /// </summary>
    public void OnConnectToLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /// <summary>
    /// Método para crear una sala de partida
    /// </summary>
    /*
    public void OnCreateRoom()
    {
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Creando sala...";

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions()
        {
            MaxPlayers = MAX_PLAYERS_INROOM
        }))
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Sala creada con éxito.";
        }
        else
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Fallo al crear la sala.";
        }
    }
    */
    
    /// <summary>
    /// Método para conectarse a una sala
    /// </summary>
    public void OnConnectToRandomRoom()
    {
        if (PhotonNetwork.JoinRandomRoom()) return;
        Debug.Log("Fallaste");
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Fallo al crear la sala.";
    }

    /// <summary>
    /// Método para abandonar la sala de la partida
    /// </summary>
    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

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

    #region PUN_CB

    /// <summary>
    /// Método ejecutado cuando se conecta al servidor
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Conectado al servidor. Conectando al lobby...";
        OnConnectToLobby();
        
    }

    /// <summary>
    /// Método ejecutado cuando se conecta a la lobby
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Conectado al lobby general.";
        Debug.Log("Hola, " + PhotonNetwork.NickName);

        SceneManager.LoadScene("MainMenu");
    }
    
    /// <summary>
    /// Método ejecutado cuando se une a una sala
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
/*
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Unido a la sala: " + PhotonNetwork.CurrentRoom.Name;
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Buscando jugadores...";

        
        if (FindObjectOfType<GameManager>().OwnerId.Equals(FindObjectOfType<PlayerInfo>().Name))
        {
            Debug.Log("Soy el jugador local y admin de la partida.");
            FindObjectOfType<NetworkCommunications>().SendPlayerInfoPackage("host");
        }
        else
        {
            Debug.Log("Soy el jugador local y usuario de la partida.");
            FindObjectOfType<NetworkCommunications>().SendPlayerInfoPackage("user");
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == MAX_PLAYERS_INROOM)
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Sala llena. Empezando partida...";
            
            FindObjectOfType<GameManager>().IsPlaying = true;

            SceneManager.LoadScene("TicTacToe_Server");
        }
        */
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        
        Debug.Log("Hola, estoy aquí");
    }

    /// <summary>
    /// Método ejecutado cuando falla el acceso a la sala
    /// </summary>
    /// <param name="returnCode">Código de error</param>
    /// <param name="message">Mensaje de error</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(returnCode + ": " + message);
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "No existen salas. Creando...";
    }

    /// <summary>
    /// Método ejecutado cuando se une un jugador a la sala
    /// </summary>
    /// <param name="newPlayer">Nuevo jugador unido</param>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Buscando jugadores...";

        if (PhotonNetwork.CurrentRoom.PlayerCount == MAX_PLAYERS_INROOM)
        {
            GameObject.FindGameObjectWithTag("Log").GetComponent<TMP_Text>().text = "Sala llena. Empezando partida...";

            FindObjectOfType<GameManager>().IsPlaying = true;

            SceneManager.LoadScene("TicTacToe_Server");
        }
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

    #endregion
    
    /*
    #region UnityCB

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #endregion

    */
}
