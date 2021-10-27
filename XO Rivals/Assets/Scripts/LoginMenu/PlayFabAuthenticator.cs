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
struct AuthObject
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
    private string _playFabPlayerIdCache;
    /// <summary>
    /// 
    /// </summary>
    private AuthObject obj;
    
    #endregion

    #region UnityCB

    private void Start()
    {
        obj = new AuthObject(false, PlayFabErrorCode.Unknown, "");
    }

    #endregion

    #region AuthMethods

    public void AuthWithPlayfab(string username, string password, LoginMode mode)
    {
        switch (mode)
        {
            case LoginMode.REGISTER:
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

        if (obj.Failed)
        {
            throw new LoginFailedException(obj.Message)
            {
                ErrorCode = obj.ErrorCode
            };
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
        obj.Failed = true;
        obj.ErrorCode = error.Error;
        obj.Message = error.GenerateErrorReport();
    }

    #endregion

    
}
