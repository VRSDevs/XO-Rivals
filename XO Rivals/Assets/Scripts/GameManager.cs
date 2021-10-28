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

    
}
