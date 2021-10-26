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
