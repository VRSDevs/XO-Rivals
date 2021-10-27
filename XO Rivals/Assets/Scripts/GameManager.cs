using System;
using System.Collections.Generic;
/*
using BrainCloud.LitJson;
using JsonReader = BrainCloud.JsonFx.Json.JsonReader;
*/
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region Variables
    
    ////////////////// SERVIDOR //////////////////
    /// <summary>
    /// 
    /// </summary>
    public NetworkController _networkController;
    /// <summary>
    /// 
    /// </summary>
    private const int MIN_PLAYERS_IN_ROON = 2;
    /// <summary>
    /// 
    /// </summary>
    private const int MAX_PLAYERS_INROOM = 2;

    ////////////////// USUARIO //////////////////
    /// <summary>
    /// 
    /// </summary>
    public string Username;

    #endregion

    #region UnityCB
    
    private void Awake()
    {
        _networkController = gameObject.AddComponent<NetworkController>();
        DontDestroyOnLoad(this);
    }

    #endregion
}
