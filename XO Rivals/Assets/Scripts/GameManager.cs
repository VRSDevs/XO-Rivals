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
        OwnerId = player.Name;
        PlayerInfoO = player;
        WhosTurn = player;
    }

    public void OnConnectToRoom(PlayerInfo player){

        PlayerInfoX = player;
    }
    
    #endregion

    #region ConversionToObjectMethods
    
    public object[] PlayerInfoToObject(char type)
    {
        switch (type)
        {
            case 'h':
                object[] objHost = new object[4];

                objHost[0] = MatchId;
                objHost[1] = OwnerId;
                objHost[2] = PlayerInfoO.Name;
                objHost[3] = WhosTurn.Name;
        
                return objHost;
            case 'p':
                object[] objPlayer = new object[1];
                
                objPlayer[0] = PlayerInfoX.Name;
                
                return objPlayer;
        }

        return null;
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="localPlayer"></param>
    /// <returns></returns>
    public object[] ToObject(PlayerInfo localPlayer)
    {
        object[] obj = new object[7];

        obj[0] = MatchId;
        obj[1] = OwnerId;
        
        if (PlayerInfoX == null)
        {
            obj[2] = localPlayer.Name.Equals(PlayerInfoO.Name) ? PlayerInfoO.Name : PlayerInfoX.Name;
        }
        else
        {
            obj[2] = localPlayer.Name.Equals(PlayerInfoX.Name) ? PlayerInfoX.Name : PlayerInfoO.Name;
        }

        if (WhosTurn != null)
        {
            obj[3] = WhosTurn.Name;
        }
        
        obj[4] = NumFilled;
        obj[5] = FilledPositions;
        obj[6] = MiniGameChosen;
        
        return obj;
    }

    #endregion
}
