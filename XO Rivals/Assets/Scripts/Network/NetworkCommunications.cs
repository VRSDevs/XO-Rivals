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

    public void SendPlayerInfoPackage(char playerType)
    {
        object[] objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(playerType);
        _View.RPC("RPCGetPlayerInfo", RpcTarget.OthersBuffered, (object)objToSend);
    }

    /// <summary>
    /// 
    /// </summary>
    public void SendMatchInfo(string type)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.MatchInfoToObject(type);
        _View.RPC("RPCUpdateMatch", RpcTarget.All, (object)objToSend);
    }

    public void SendEndMatchInfo(string type, string winner)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.EndMatchInfoToObject(type, winner);
        _View.RPC("RPCEndMatch", RpcTarget.All, (object)objToSend);
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
        switch (obj[0] as string)
        {
            case "OppWon":
                
                Debug.Log("RPC oponente ganó");
                
                FindObjectOfType<GameManager>().WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().NumFilled = (int)obj[2];
                FindObjectOfType<GameManager>().FilledPositions[
                    (int)obj[3], (int)obj[4]
                ] = (int)obj[5];
                

                FindObjectOfType<ScreenManager>().UpdateBoard(
                    (int)obj[3], 
                    (int)obj[4], 
                    (string)obj[6]
                );
                
                FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[7];

                FindObjectOfType<GameManager>().turnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
            case "OppLost":
                
                Debug.Log("RPC oponente perdió");
                
                FindObjectOfType<GameManager>().WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().MiniGameChosen = (int)obj[2];
                
                FindObjectOfType<GameManager>().turnMoment = 0;

                FindObjectOfType<ButtonsScript>().UpdateTurn();
                
                break;
        }
    }
    
    public void RPCEndMatch(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "win":
            case "defeat":

                if (obj[1].Equals(GameObject.Find("PlayerObject").GetComponent<PlayerInfo>().Name))
                {
                    FindObjectOfType<EndGameScript>().ShowMatchVictory();
                }
                else
                {
                    FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                }

                break;
            case "draw":
                
                FindObjectOfType<EndGameScript>().ShowMatchDraw();
                
                break;
        }
    }

    #endregion
}
