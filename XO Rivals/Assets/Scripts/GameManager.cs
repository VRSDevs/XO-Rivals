using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] public TMP_Text log;
    
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
    /// <summary>
    /// 
    /// </summary>
    [NonSerialized] public bool IsWebGLMobile;

    #endregion

    #region UnityCB
    
    private void Awake()
    {
        _networkController = gameObject.AddComponent<NetworkController>();
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
