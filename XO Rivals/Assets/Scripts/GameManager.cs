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
    public NetworkCommunications _NetworkCommunications;
    
    ////////////////// PARTIDA //////////////////
    
    
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

    #endregion

    #region UnityCB
    
    private void Awake()
    {
        _networkController = gameObject.AddComponent<NetworkController>();
        _NetworkCommunications = gameObject.AddComponent<NetworkCommunications>();
        
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
    
    #endregion

    public void OnConnectToRoom()
    {
        _networkController.ConnectToRandomRoom();
    }

    /// <summary>
    /// Método para abandonar una sala en Photon
    /// </summary>
    public void OnLeaveRoom()
    {
        _networkController.DisconnectFromRoom();
    }

    #region UpdateMethods
    
    /// <summary>
    /// Método para actualizar el nick del cliente en Photon
    /// </summary>
    /// <param name="nick">Nick del cliente</param>
    public void UpdatePhotonNick(string nick)
    {
        _networkController.SetNickName(nick);
    }

    #endregion

    #region ConversionToObjectMethods
    
    public object[] PlayerInfoToObject(string type)
    {
        switch (type)
        {
            case "host":
                object[] objHost = new object[5];

                objHost[0] = type;
                objHost[1] = MatchId;
                objHost[2] = OwnerId;
                objHost[3] = PlayerInfoO.Name;
                objHost[4] = WhosTurn;
        
                return objHost;
            case "user":
                object[] objPlayer = new object[2];
                
                objPlayer[0] = type;
                objPlayer[1] = PlayerInfoX.Name;
                
                return objPlayer;
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
