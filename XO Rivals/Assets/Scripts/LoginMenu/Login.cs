using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Login : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public GameManager _gameManager;
    
    /// <summary>
    /// 
    /// </summary>
    private bool IsConnecting;
    
    /// <summary>
    /// Mínimo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MIN_CHARS = 3;
    /// <summary>
    /// Máximo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MAX_CHARS = 24;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_InputField UsernameInput;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_InputField PasswordInput;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_Text LoginInfo;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        UsernameInput.characterLimit = MAX_CHARS;
        PasswordInput.characterLimit = MAX_CHARS;

        LoginInfo.text = "";
        
        //OnReconnect();
    }

    #endregion

    #region LoginMethods

    public void OnConnect()
    {
        if (!IsConnecting && ValidateLoginInputs())
        {
            IsConnecting = true;
            LoginInfo.text = "Conectando...";

            if (_gameManager._networkController.OnConnectToServer())
            {
                LoginInfo.text = "Conectado.";
                _gameManager.Username = UsernameInput.text;
                _gameManager._networkController.SetNickName(UsernameInput.text);
            }
            else
            {
                LoginInfo.text = "Error al conectar.";
            }
        }
    }

    private void OnReconnect()
    {
  
    }

    private void OnAuthentication(string response, object cbObject)
    {
        /*
        var data = JsonMapper.ToObject(response)["data"];
        _gameManager.UserId = data["profileId"].ToString();
        _gameManager.Username = data["playerName"].ToString();
        
        PlayerPrefs.SetString(_gameManager.Username + "_hasAuthenticated", "true");
        
        bool isNewUser = data["newUser"].ToString().Equals("true");

        if (isNewUser)
        {
            SetupPlayer();
        }
        */
    }

    #endregion

    #region CheckMethods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool ValidateLoginInputs()
    {
        //
        UsernameInput.text = UsernameInput.text.Trim();
        PasswordInput.text = PasswordInput.text.Trim();
        
        //
        if (UsernameInput.text.Length < MIN_CHARS /*|| PasswordInput.text.Length < MIN_CHARS*/)
        {
            LoginInfo.text = "Longitud no correcta, mínimo " + MIN_CHARS;
            return false;
        }

        return true;
    }
    

    #endregion

    #region OtherMethods

    private void SetupPlayer()
    {
        /*
        _gameManager.Server.PlayerStateService.UpdateName(UsernameInput.text,
            (jsonResponse, o) =>
            {
                Debug.Log("Inició sesión");
            });
            */
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void ShowPasswordField()
    {
        switch (PasswordInput.contentType)
        {
            case TMP_InputField.ContentType.Standard:
                Debug.Log("Adius");
                PasswordInput.contentType = TMP_InputField.ContentType.Password;
                break;
            case TMP_InputField.ContentType.Password:
                Debug.Log("Hula");
                PasswordInput.contentType = TMP_InputField.ContentType.Standard;
                break;
        }
        
        PasswordInput.ForceLabelUpdate();
    }

    #endregion
}
