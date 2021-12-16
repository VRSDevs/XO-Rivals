using System;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using CustomAuthenticationType = Photon.Chat.CustomAuthenticationType;
using Object = System.Object;

#region Structs

/// <summary>
/// 
/// </summary>
public struct AuthObject
{
    /// <summary>
    /// 
    /// </summary>
    public bool Failed;
    /// <summary>
    /// 
    /// </summary>
    public PlayFabErrorCode ErrorCode;
    /// <summary>
    /// 
    /// </summary>
    public string Message;

    #region Constructors

    public AuthObject(bool status, PlayFabErrorCode error, string message)
    {
        Failed = status;
        ErrorCode = error;
        Message = message;
    }

    #endregion
}

#endregion


public class PlayFabAuthenticator : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// ID del usuario en el proceso de autentificación
    /// </summary>
    public string playFabPlayerIdCache = "";
    /// <summary>
    /// 
    /// </summary>
    public AuthObject Obj;

    private bool Authenticated;
    
    #endregion

    #region UnityCB

    private void Start()
    {
        Authenticated = false;
        Obj = new AuthObject(false, PlayFabErrorCode.Unknown, "");
    }

    #endregion

    #region AuthMethods

    public void AuthWithPlayfab(string username, string password, LoginMode mode)
    {
        switch (mode)
        {
            case LoginMode.Register:
                RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
                {
                    Username = username,
                    Password = password,
                    RequireBothUsernameAndEmail = false
                };

                PlayFabClientAPI.RegisterPlayFabUser(
                    registerRequest,
                    result =>
                    {
                        Authenticated = true;
                    },
                    OnError
                );

                break;
            case LoginMode.Login:
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
        playFabPlayerIdCache = result.PlayFabId;

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
        
        customAuth.AddAuthParameter("username", playFabPlayerIdCache);
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = customAuth;

        Authenticated = true;
    }

    private void OnError(PlayFabError error)
    {
        Obj.Failed = true;
        Obj.ErrorCode = error.Error;
        Obj.Message = error.GenerateErrorReport();

        Authenticated = true;
    }

    #endregion

    public bool IsAuthenticated()
    {
        return Authenticated;
    }

    public void Reset()
    {
        playFabPlayerIdCache = "";
        Authenticated = false;
        Obj = new AuthObject(false, PlayFabErrorCode.Unknown, "");
    }
}
