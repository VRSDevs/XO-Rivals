using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Login : GameManager
{
    #region Variables

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
            
            /*
            
            Server.AuthenticateUniversal(Username.text, Password.text, true, OnAuthenticate,
                (status, code, error, cbObject) =>
                {
                    IsConnecting = false;
                    LoginInfo.text = "Error";
                });
            */
        }
    }

    private void OnReconnect()
    {
  
    }

    private void OnAuthenticate()
    {
        
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
        if (UsernameInput.text.Length < MIN_CHARS || PasswordInput.text.Length < MIN_CHARS)
        {
            LoginInfo.text = "Longitud no correcta, mínimo " + MIN_CHARS;
            return false;
        }

        return true;
    }
    

    #endregion

    #region OtherMethods

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
