using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    
    
    #region ConnectMethods

    /// <summary>
    /// 
    /// </summary>
    public bool OnConnectToServer()
    {
        if (PhotonNetwork.ConnectUsingSettings())
        {
            return true;
        }

        return false;
    }

    public void OnConnectToLobby()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    #endregion

    #region PUN_CB

    /// <summary>
    /// 
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        Debug.Log("Conectado al servidor.");
        OnConnectToLobby();
        
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        Debug.Log("Conectado al lobby general.");
        Debug.Log("Hola, " + PhotonNetwork.NickName);
    }
    
    /*
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }    
     */


    #endregion

    #region UpdateNetworkVars

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nick"></param>
    public void SetNickName(string nick)
    {
        PhotonNetwork.NickName = nick;
    }

    #endregion
    
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
