using System;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] public TMP_Text log;
    
    ////////////////// SERVIDOR //////////////////
    /// <summary>
    /// Referencia a funciones del servidor
    /// </summary>
    private NetworkController _networkController;
    /// <summary>
    /// 
    /// </summary>
    public NetworkCommunications _networkCommunications;
    
    ////////////////// PARTIDA //////////////////
    /// <summary>
    /// Lista de partidas del jugador
    /// </summary>
    public Dictionary<string, Match> PlayerMatches;

    /// <summary>
    /// ID de la partida
    /// </summary>
    public string MatchId;
    /// <summary>
    /// ID del dueño de la partida
    /// </summary>
    public string OwnerId;
    /// <summary>
    /// Información del jugador O
    /// </summary>
    public PlayerInfo PlayerInfoO;
    /// <summary>
    /// Información del jugador X
    /// </summary>
    public PlayerInfo PlayerInfoX;
    /// <summary>
    /// Información del jugador del turno
    /// </summary>
    public string WhosTurn;

    /// <summary>
    /// Información del momento del turno en el que esta el jugador
    /// turnMoment = 0 -> Toca colocar ficha
    /// turnMoment = 1 -> Toca jugar el minijuego
    /// turnMoment = 2 -> Toca elegir minijuego
    /// </summary>
    public int turnMoment;      
    /// <summary>
    /// Numero de fichas colocadas
    /// </summary>
    public int NumFilled;
    /// <summary>
    /// Posicion de fichas colocadas
    /// </summary>
    public int[,] FilledPositions;    
    /// <summary>
    /// Array de fichas colocadas
    /// </summary>
    public List<GameObject> Chips;
    /// <summary>
    /// Minijuego elegido
    /// </summary>
    public int MiniGameChosen;
    
    ////////////////// USUARIO //////////////////
    /// <summary>
    /// ¿En qué versión de WebGL está?
    /// </summary>
    [NonSerialized] public bool IsWebGLMobile;
    /// <summary>
    /// ¿Está jugando?
    /// </summary>
    [NonSerialized] public bool IsPlaying;

    /// <summary>
    /// 
    /// </summary>
    [NonSerialized] public bool Matchmaking;
    
    #endregion

    #region UnityCB
    
    private void Awake()
    {
        _networkController = gameObject.AddComponent<NetworkController>();
        _networkCommunications = gameObject.AddComponent<NetworkCommunications>();

        PlayerMatches = new Dictionary<string, Match>();
        Matchmaking = false;
        IsPlaying = false;
        
        DontDestroyOnLoad(this);
    }

    #endregion

    #region ConnectionMethods

    /// <summary>
    /// Método para conectarse al servidor de Photon
    /// </summary>
    /// <returns>Devuelve "true" si el cliente pudo establecer conexión con el servidor</returns>
    public bool OnConnectToServer()
    {
        return _networkController.ConnectToServer();
    }
    
    /// <summary>
    /// Método para conectarse a una sala en Photon
    /// </summary>
    public void OnConnectToRoom()
    {
        _networkController.ConnectToRandomRoom();
    }

    /// <summary>
    /// Método para conectarse a la lobby general
    /// </summary>
    public void OnConnectToLobby()
    {
        _networkController.ConnectToLobby();
    }

    /// <summary>
    /// Método para abandonar una sala en Photon
    /// </summary>
    public void OnLeaveRoom()
    {
        Debug.Log("a");
        _networkController.DisconnectFromRoom();
    }
    
    #endregion

    #region MatchMethods

    /// <summary>
    /// Método para configurar partida y enviar los datos necesarios
    /// </summary>
    /// <param name="playerType">Tipo del jugador (ficha)</param>
    public void SetupMatch(char playerType)
    {
        PlayerMatches.Add(PhotonNetwork.CurrentRoom.Name, new Match());
        
        switch (playerType)
        {
            case 'O':
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName = FindObjectOfType<PlayerInfo>().Name;
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = FindObjectOfType<PlayerInfo>().Name;

                break;
            case 'X':
                PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName = FindObjectOfType<PlayerInfo>().Name;

                break;
        }
        
        _networkCommunications.SendPlayerInfoPackage(playerType);
    }

    #endregion

    #region UpdateMethods
    
    /// <summary>
    /// Método para actualizar el nick del cliente en Photon
    /// </summary>
    /// <param name="nick">Nick del cliente</param>
    public void UpdatePhotonNick(string nick)
    {
        _networkController.SetNickName(nick);
    }
    
    /// <summary>
    /// Método para actualizar el estado del jugador para empezar la partida
    /// </summary>
    public void UpdateReadyStatus()
    {
        _networkController.UpdateReadyStatus();
    }

    #endregion

    #region ConversiontMethods
    
    /// <summary>
    /// Método para convertir los datos del jugador en un objeto a enviar
    /// </summary>
    /// <param name="playerType">Tipo del jugador (ficha)</param>
    /// <returns>Objeto de datos</returns>
    public object[] PlayerInfoToObject(char playerType)
    {
        switch (playerType)
        {
            case 'O':
                object[] objO = new object[3];

                objO[0] = playerType;
                objO[1] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName;
                objO[2] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn;

                return objO;
            case 'X':
                object[] objX = new object[2];

                objX[0] = playerType;
                objX[1] = PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName;

                return objX;
        }

        return null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="localPlayer"></param>
    /// <returns></returns>
    public object[] MatchInfoToObject(string type)
    {
        switch (type)
        {
            case "OppWon":
                object[] objOppWon = new object[8];

                objOppWon[0] = type;
                objOppWon[1] = WhosTurn;
                objOppWon[2] = NumFilled;
                objOppWon[3] = FindObjectOfType<ButtonsScript>().col;
                objOppWon[4] = FindObjectOfType<ButtonsScript>().row;
                objOppWon[5] = FilledPositions[
                    FindObjectOfType<ButtonsScript>().col,
                    FindObjectOfType<ButtonsScript>().row
                ];
                objOppWon[6] = FindObjectOfType<ButtonsScript>().SelectedTile;
                objOppWon[7] = MiniGameChosen;
        
                return objOppWon;
            case "OppLost":
                object[] objOppLost = new object[3];
                
                Debug.Log(WhosTurn);
                
                objOppLost[0] = type;
                objOppLost[1] = WhosTurn;
                objOppLost[2] = MiniGameChosen;
                
                return objOppLost;
        }

        return null;
    }

    public object[] EndMatchInfoToObject(string type, string winner)
    {
        object[] obj;
        
        switch (type)
        {
            case "win":
            case "defeat":
                obj = new object[2];

                obj[0] = type;
                obj[1] = winner;

                return obj;
            case "draw":
                obj = new object[1];
                
                obj[0] = type;
                
                return obj;
        }

        return null;
    }

    #endregion
}
