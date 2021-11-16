using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchmakingController : IMatchmakingCallbacks
{
    #region Vars

    private LoadBalancingClient _loadBalancingClient;
    
    /// <summary>
    /// Jugadores mínimos en la sala
    /// </summary>
    private const int MIN_PLAYERS_IN_ROON = 2;
    /// <summary>
    /// Jugadores máximos en la sala
    /// </summary>
    private const int MAX_PLAYERS_INROOM = 2;    

    #endregion

    #region RoomsMethods

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = MAX_PLAYERS_INROOM,
        };

        EnterRoomParams enterParams = new EnterRoomParams
        {
            RoomName = "Sala 1",
            RoomOptions = options
        };

        _loadBalancingClient.OpCreateRoom(enterParams);
    }
    

    #endregion

    #region IMatchmakingCallbacks

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }

    #endregion
    
    
}
