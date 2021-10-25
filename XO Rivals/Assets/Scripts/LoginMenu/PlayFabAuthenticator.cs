using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using CustomAuthenticationType = Photon.Chat.CustomAuthenticationType;

public class PlayFabAuthenticator : MonoBehaviour
{
    private string _playFabPlayerIdCache;

    public void AuthWithPlayfab(string username, string password, LoginMode mode)
    {
        // TO DO
        switch (mode)
        {
            case LoginMode.REGISTER:
                RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
                {
                    Username = username,
                    Password = password
                };

                PlayFabClientAPI.RegisterPlayFabUser(
                    registerRequest,
                    result =>
                    {
                        
                    },
                    OnError
                    );
                
                break;
            case LoginMode.LOGIN:
                LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest();
                loginRequest.Username = username;
                loginRequest.Password = password;
                
                PlayFabClientAPI.LoginWithPlayFab(
                        loginRequest,
                        RequestToken,
                        OnError
                    );
                
                break;
        }
    }

    private void RequestToken(LoginResult result)
    {
        _playFabPlayerIdCache = result.PlayFabId;

        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        
        PlayFabClientAPI.GetPhotonAuthenticationToken(
                request,
                AuthWithPhoton,
                OnError
            );
    }

    private void AuthWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        var customAuth = new AuthenticationValues
        {
            AuthType = Photon.Realtime.CustomAuthenticationType.Custom
        };
        
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = customAuth;
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("PlayFab + Photon: " + error.GenerateErrorReport());
    }
}
