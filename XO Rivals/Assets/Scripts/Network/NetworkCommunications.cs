using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCommunications : MonoBehaviourPun
{
    private PhotonView _View;
    
    private void Start()
    {
        _View = gameObject.AddComponent<PhotonView>();
        _View.ViewID = 1;
    }

    public void SendPlayerInfoPackage(string type)
    {
        object[] objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(type);
        _View.RPC("RPCGetPlayerInfo", RpcTarget.OthersBuffered, (object)objToSend);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendMatchInfo(string type)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.MatchInfoToObject(type);
        Debug.Log("Vamos a mandar: " + objToSend);
        _View.RPC("RPCUpdateMatch", RpcTarget.Others, (object)objToSend);
    }
    
    #region RPCMethods
    
    [PunRPC]
    public void RPCGetPlayerInfo(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "host":
                FindObjectOfType<GameManager>().MatchId = obj[0] as string;
                FindObjectOfType<GameManager>().OwnerId = obj[1] as string;

                if (FindObjectOfType<GameManager>().PlayerInfoO == null)
                {
                    FindObjectOfType<GameManager>().PlayerInfoO = gameObject.AddComponent<PlayerInfo>();
                    FindObjectOfType<GameManager>().PlayerInfoO.Name = obj[2] as string;
                }

                if (FindObjectOfType<GameManager>().WhosTurn == "")
                {
                    FindObjectOfType<GameManager>().WhosTurn = obj[3] as string;
                }
                
                break;
            case "user":

                if (FindObjectOfType<GameManager>().PlayerInfoX == null)
                {
                    FindObjectOfType<GameManager>().PlayerInfoX = gameObject.AddComponent<PlayerInfo>();
                    FindObjectOfType<GameManager>().PlayerInfoX.Name = obj[1] as string;
                }
                
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    [PunRPC]
    public void RPCUpdateMatch(object[] obj)
    {
        Debug.Log("RPC recibido");

        switch (obj[0] as string)
        {
            case "OppWon":
                
                FindObjectOfType<GameManager>().WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().NumFilled = (int)obj[2];
                FindObjectOfType<GameManager>().FilledPositions = (int[,])obj[3];
                FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[4];

                FindObjectOfType<GameManager>().turnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
            case "OppLost":
                
                FindObjectOfType<GameManager>().WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[2];
                
                FindObjectOfType<GameManager>().turnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
        }
    }

    #endregion
}
