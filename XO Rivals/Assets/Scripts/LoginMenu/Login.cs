using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum LoginMode
{
    NONE,
    REGISTER,
    LOGIN
}

public class Login : MonoBehaviour
{
    #region Variables
    
    /// <summary>
    /// 
    /// </summary>
    [NonSerialized] public LoginMode Mode;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public GameManager _gameManager;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public PlayFabAuthenticator Authenticator;
    
    /// <summary>
    /// 
    /// </summary>
    private bool IsConnecting;
    
    /// <summary>
    /// Mínimo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MIN_CHARS = 6;
    /// <summary>
    /// Máximo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MAX_CHARS = 100;
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_InputField R_UsernameInput;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_InputField R_PasswordInput;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] public TMP_Text RegisterInfo;

    /// <summary>
    /// Referencia (en Login) al InputField del nombre de usuario
    /// </summary>
    [SerializeField] public TMP_InputField L_UsernameInput;
    /// <summary>
    /// Referencia (en Login) al InputField de la constraseña
    /// </summary>
    [SerializeField] public TMP_InputField L_PasswordInput;
    /// <summary>
    /// Referencia al log de información del login
    /// </summary>
    [SerializeField] public TMP_Text LoginInfo;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        L_UsernameInput.characterLimit = MAX_CHARS;
        L_PasswordInput.characterLimit = MAX_CHARS;
        R_UsernameInput.characterLimit = MAX_CHARS;
        R_PasswordInput.characterLimit = MAX_CHARS;

        RegisterInfo.text = "";
        LoginInfo.text = "";
    }

    #endregion

    #region LoginMethods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    public void OnConnect()
    {
        if (!IsConnecting && ValidateInputs())
        {
            IsConnecting = true;

            switch (Mode)
            {
                case LoginMode.REGISTER:
                    
                    RegisterInfo.text = "Conectando...";
                    
                    if (OnAuthentication(R_UsernameInput.text, R_PasswordInput.text))
                    {
                        if (_gameManager._networkController.OnConnectToServer())
                        {
                            RegisterInfo.text = "Conectado.";
                            _gameManager.Username = R_UsernameInput.text;
                            _gameManager._networkController.SetNickName(R_UsernameInput.text);
                        }
                        else
                        {
                            LoginInfo.text = "Error al conectar.";
                            IsConnecting = false;
                        }
                    }
                    
                    break;
                case LoginMode.LOGIN:
                    
                    LoginInfo.text = "Conectando...";
                    
                    if (OnAuthentication(L_UsernameInput.text, L_PasswordInput.text))
                    {
                        if (_gameManager._networkController.OnConnectToServer())
                        {
                            LoginInfo.text = "Conectado.";
                            _gameManager.Username = L_UsernameInput.text;
                            _gameManager._networkController.SetNickName(L_UsernameInput.text);
                        }
                        else
                        {
                            LoginInfo.text = "Error al conectar.";
                            IsConnecting = false;
                        }
                    }
                    
                    break;
            }
        }
    }

    private bool OnAuthentication(string username, string password)
    {
        try
        {
            Authenticator.AuthWithPlayfab(username, password, Mode);
            return true;
        }
        catch (LoginFailedException e)
        {
            Debug.Log("[SISTEMA]: " + e.Message + "(" + e.ErrorCode + ")");
            return false;
        }
    }

    #endregion

    #region CheckMethods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool ValidateInputs()
    {
        switch (Mode)
        {
            case LoginMode.REGISTER:
                //
                R_UsernameInput.text = R_UsernameInput.text.Trim();
                R_PasswordInput.text = R_PasswordInput.text.Trim();
        
                //
                if (R_UsernameInput.text.Length < MIN_CHARS || R_UsernameInput.text.Length < MIN_CHARS)
                {
                    RegisterInfo.text = "Longitud no correcta, mínimo " + MIN_CHARS;
                    return false;
                }

                return true;
            case LoginMode.LOGIN:
                //
                L_UsernameInput.text = L_UsernameInput.text.Trim();
                L_PasswordInput.text = L_PasswordInput.text.Trim();
        
                //
                if (L_UsernameInput.text.Length < MIN_CHARS || L_UsernameInput.text.Length < MIN_CHARS)
                {
                    LoginInfo.text = "Longitud no correcta, mínimo " + MIN_CHARS;
                    return false;
                }

                return true;
        }

        return false;
    }
    

    #endregion

    #region OtherMethods

    public void UpdateLoginMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Mode = LoginMode.NONE;
                break;
            case 1:
                Mode = LoginMode.REGISTER;
                break;
            case 2:
                Mode = LoginMode.LOGIN;
                break;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void ShowPasswordField()
    {
        switch (Mode)
        {
            case LoginMode.REGISTER:
                switch (R_PasswordInput.contentType)
                {
                    case TMP_InputField.ContentType.Standard:
                        R_PasswordInput.contentType = TMP_InputField.ContentType.Password;
                        break;
                    case TMP_InputField.ContentType.Password:
                        R_PasswordInput.contentType = TMP_InputField.ContentType.Standard;
                        break;
                }
        
                R_PasswordInput.ForceLabelUpdate();
                
                break;
            case LoginMode.LOGIN:
                switch (L_PasswordInput.contentType)
                {
                    case TMP_InputField.ContentType.Standard:
                        L_PasswordInput.contentType = TMP_InputField.ContentType.Password;
                        break;
                    case TMP_InputField.ContentType.Password:
                        L_PasswordInput.contentType = TMP_InputField.ContentType.Standard;
                        break;
                }
        
                L_PasswordInput.ForceLabelUpdate();
                
                break;
        }
    }

    #endregion
}
