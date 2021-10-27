using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;


public class QuickMatchExample : IMatchmakingCallbacks
{



    [SerializeField]
    private byte numPlayers = 2;
    private LoadBalancingClient loadBalancingClient;


    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = numPlayers;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomOptions = roomOptions;
        loadBalancingClient.OpCreateRoom(enterRoomParams);
    }


    private void QuickMatch()
    {
        loadBalancingClient.OpJoinRandomOrCreateRoom(null, null);
    }


    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }

}
