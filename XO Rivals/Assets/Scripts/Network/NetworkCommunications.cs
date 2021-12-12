using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCommunications : MonoBehaviourPun
{
    #region Vars

    private PhotonView _View;

    #endregion
    
    #region UnityCB

    private void Start()
    {
        _View = PhotonView.Get(this);
    }

    #endregion
    
    #region SendingMethods
    
    /// <summary>
    /// Método para enviar información del jugador al oponente
    /// </summary>
    /// <param name="playerType">Tipo del jugador (en partida)</param>
    public void SendPlayerInfoPackage(string playerType)
    {
        object[] objToSend = FindObjectOfType<GameManager>().PlayerInfoToObject(playerType);
        _View.RPC("PlayerInfoRPC", RpcTarget.Others, (object)objToSend);

    }
    
    /// <summary>
    /// Método para enviar información del estado de la partida al oponente
    /// </summary>
    public void SendMatchInfo(string type)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.MatchInfoToObject(type);
        _View.RPC("RPCUpdateMatch", RpcTarget.OthersBuffered, (object)objToSend);
    }
    
    /// <summary>
    /// Método para enviar información de la finalización de la partida
    /// </summary>
    /// <param name="type"></param>
    /// <param name="winner"></param>
    public void SendEndMatchInfo(string type, string winner)
    {
        object[] objToSend = FindObjectOfType<ButtonsScript>().gameState.EndMatchInfoToObject(type, winner);
        _View.RPC("RPCEndMatch", RpcTarget.OthersBuffered, (object)objToSend);
    }
    
    #endregion

    #region RPCMethods
    
    /// <summary>
    /// RPC recibido en el usuario con la información del jugador contrario
    /// </summary>
    /// <param name="obj">Objeto con la información</param>
    [PunRPC]
    public void PlayerInfoRPC(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "O":
                Debug.Log("RPC del jugador O");

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].OpponentId = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerOName = obj[2] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[3] as string;

                FindObjectOfType<GameManager>().SetReadyStatus();

                break;
            case "X":
                Debug.Log("RPC del jugador X");

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].OpponentId = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].PlayerXName = obj[2] as string;

                FindObjectOfType<GameManager>().SetReadyStatus();

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
                Debug.Log(obj[0] as string);
                Debug.Log(obj[1] as string);
                Debug.Log((int)obj[2]);
                Debug.Log((int)obj[3]);
                Debug.Log((int)obj[4]);
                Debug.Log((int)obj[5]);
                Debug.Log((string)obj[6]);
                Debug.Log(obj[7] as string);



                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].NumFilled = (int)obj[2];
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].FilledPositions[
                    (int)obj[3], (int)obj[4]
                ] = (int)obj[5];
                

                FindObjectOfType<ScreenManager>().UpdateBoard(
                    (int)obj[3], 
                    (int)obj[4], 
                    (string)obj[6]
                );
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen = (int)obj[7];

                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().startGame();
                FindObjectOfType<ButtonsScript>().updateIconTurn(true);
                FindObjectOfType<ButtonsScript>().colocarFichas();
                FindObjectOfType<ButtonsScript>().CheckVictory();




                break;
            case "OppLost":
                
                Debug.Log("RPC oponente perdió");
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].WhosTurn = obj[1] as string;
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].MiniGameChosen = (int)obj[2];
                
                FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].TurnMoment = 0;

                FindObjectOfType<ButtonsScript>().startGame();
                FindObjectOfType<ButtonsScript>().updateIconTurn(true);

                break;
        }
    }
    
    /// <summary>
    /// RPC recibido en el usuario con la información del final de la partida
    /// </summary>
    /// <param name="obj">Objeto con la información</param>
    [PunRPC]
    public void RPCEndMatch(object[] obj)
    {
        switch (obj[0] as string)
        {
            case "WN":
            case "DF":
                if (obj[1].Equals(GameObject.Find("PlayerObject").GetComponent<PlayerInfo>().Name))//RECIBE QUIEN GANA
                {
                    FindObjectOfType<EndGameScript>().ShowMatchVictory();
                }
                else
                {
                     FindObjectOfType<EndGameScript>().ShowMatchDefeat();
                     FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].SetIsEnded();
                }

                break;
            case "DW":
                    FindObjectOfType<EndGameScript>().ShowMatchDraw();

                break;
            case "SR":
                    FindObjectOfType<EndGameScript>().ShowSurrenderVictory();
                break;

        }
    }

    #endregion
}
