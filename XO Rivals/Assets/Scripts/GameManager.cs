using System;
using System.Collections.Generic;
using BrainCloud.LitJson;
using UnityEngine;
using JsonReader = BrainCloud.JsonFx.Json.JsonReader;

public class GameManager : MonoBehaviour
{
    #region Variables
    
    ////////////////// SERVIDOR //////////////////
    /// <summary>
    /// 
    /// </summary>
    public BrainCloudWrapper Server;
    
    ////////////////// USUARIO //////////////////
    /// <summary>
    /// 
    /// </summary>
    public string Username;
    /// <summary>
    /// 
    /// </summary>
    public string UserId;

    #endregion
}
