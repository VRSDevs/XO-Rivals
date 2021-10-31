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
    public NetworkController _networkController;
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
    public PlayerInfo WhosTurn;

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
    
    #region OtherMethods

    public void updateLog(string s)
    {
        log.text = s;
    }
    
    #endregion

    #region Photon
    
    public void OnCreateRoom(PlayerInfo player){
        //Match ID y OwnerID for beta version
        PlayerInfoO = player;
        WhosTurn = player;
    }

    public void OnConnectToRoom(PlayerInfo player){

        PlayerInfoX = player;
    }
    
    #endregion

    #region OtherMethods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="localPlayer"></param>
    /// <returns></returns>
    public Package ToPackage(PlayerInfo localPlayer)
    {
        Package pck = new Package();

        pck.MatchId = MatchId;
        pck.OwnerId = OwnerId;

        if (PlayerInfoX == null)
        {
            pck.Player = localPlayer.Name.Equals(PlayerInfoO.Name) ? PlayerInfoO : PlayerInfoX;
        }
        else
        {
            pck.Player = localPlayer.Name.Equals(PlayerInfoX.Name) ? PlayerInfoX : PlayerInfoO;
        }
        
        pck.WhosTurn = WhosTurn;
        pck.NumFilled = NumFilled;
        pck.FilledPositions = FilledPositions;
        pck.Chips = Chips;
        pck.MinigameChosen = MiniGameChosen;
        
        return pck;
    }

    #endregion
}
