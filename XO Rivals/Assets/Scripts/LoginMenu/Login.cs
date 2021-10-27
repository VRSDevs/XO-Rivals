using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Modo de inicio de sesión activo
///     NONE - Ninguno
///     REGISTER - Registrarse
///     LOGIN - Iniciar sesión
/// </summary>
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
    
    ////////////////// REFERENCIAS A CLASES //////////////////
    /// <summary>
    /// Referencia a la clase GameManager
    /// </summary>
    [SerializeField] public GameManager _gameManager;
    /// <summary>
    /// Referencia a la clase de autentificación de inicio de sesión
    /// </summary>
    [SerializeField] public PlayFabAuthenticator Authenticator;
    
    ////////////////// REFERENCIAS A GAMEOBJECTS //////////////////
    /// <summary>
    /// Referencia (en Registro) al InputField del nombre de usuario
    /// </summary>
    [SerializeField] public TMP_InputField R_UsernameInput;
    /// <summary>
    /// Referencia (en Registro) al InputField de la constraseña
    /// </summary>
    [SerializeField] public TMP_InputField R_PasswordInput;
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
    [SerializeField] public TMP_Text Log;
    
    ////////////////// INICIO DE SESIÓN //////////////////
    /// <summary>
    /// Modo del inicio de sesión
    /// </summary>
    [NonSerialized] public LoginMode Mode;
    /// <summary>
    /// ¿Se está conectando el usuario?
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

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        // Limitación de caracteres máximos de los inputs
        L_UsernameInput.characterLimit = MAX_CHARS;
        L_PasswordInput.characterLimit = MAX_CHARS;
        R_UsernameInput.characterLimit = MAX_CHARS;
        R_PasswordInput.characterLimit = MAX_CHARS;

        // Limpieza del log
        Log.text = "";
    }

    #endregion

    #region LoginMethods

    /// <summary>
    /// Método para establecer conexión
    /// </summary>
    public void OnConnect()
    {
        if (!IsConnecting && ValidateInputs())
        {
            IsConnecting = true;

            string username = "", password = "";

            switch (Mode)
            {
                case LoginMode.REGISTER:
                    username = R_UsernameInput.text;
                    password = R_PasswordInput.text;
                    break;
                case LoginMode.LOGIN:
                    username = L_UsernameInput.text;
                    password = L_PasswordInput.text;
                    break;
            }
            
            OnAuthentication(username, password);
            StartCoroutine(OnEstablishConnection(username, password, Mode));
        }
    }

    /// <summary>
    /// Método para realizar la autentificación de inicio de sesión
    /// </summary>
    /// <param name="username">Nombre de usuario</param>
    /// <param name="password">Contraseña</param>
    private void OnAuthentication(string username, string password)
    {
        Log.text = "Validando credenciales...";
        Authenticator.AuthWithPlayfab(username, password, Mode);
    }

    private IEnumerator OnEstablishConnection(string username, string password, LoginMode mode)
    {
        yield return new WaitUntil(Authenticator.IsAuthenticated);

        try
        {
            if (Authenticator.Obj.Failed)
            {
                throw new LoginFailedException(Authenticator.Obj.Message)
                {
                    ErrorCode = Authenticator.Obj.ErrorCode
                };
            }
            
            Log.text = "Conectando...";
            
            if (_gameManager._networkController.OnConnectToServer())
            {
                Log.text = "Conectado.";
                _gameManager.Username = username;
                _gameManager._networkController.SetNickName(username);
            }
            else
            {
                Log.text = "Error al conectar.";
                IsConnecting = false;
            }
        }
        catch (LoginFailedException e)
        {
            Debug.Log("[SISTEMA]: " + e.Message + ". (" + e.ErrorCode + ")");
        }
    }

    #endregion

    #region CheckMethods

    /// <summary>
    /// Método para validar la longitud de los inputs
    /// </summary>
    /// <returns>¿Inputs válidos?</returns>
    private bool ValidateInputs()
    {
        switch (Mode)
        {
            case LoginMode.REGISTER:
                // Eliminación de espacios en la cadena de caracteres
                R_UsernameInput.text = R_UsernameInput.text.Trim();
                R_PasswordInput.text = R_PasswordInput.text.Trim();
                
                if (R_UsernameInput.text.Length < MIN_CHARS || R_UsernameInput.text.Length < MIN_CHARS)
                {
                    Log.text = "Longitud no correcta, mínimo " + MIN_CHARS;
                    return false;
                }

                return true;
            case LoginMode.LOGIN:
                // Eliminación de espacios en la cadena de caracteres
                L_UsernameInput.text = L_UsernameInput.text.Trim();
                L_PasswordInput.text = L_PasswordInput.text.Trim();
                
                if (L_UsernameInput.text.Length < MIN_CHARS || L_UsernameInput.text.Length < MIN_CHARS)
                {
                    Log.text = "Longitud no correcta, mínimo " + MIN_CHARS;
                    return false;
                }

                return true;
        }

        return false;
    }
    
    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para actualizar la variable de modo de inicio sesión
    /// </summary>
    /// <param name="mode">Código del modo</param>
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
    /// Método para mostrar la contraseña
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
        
                // Método para forzar la actualización de muestra del input
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
        
                // Método para forzar la actualización de muestra del input
                L_PasswordInput.ForceLabelUpdate();
                
                break;
        }
    }

    #endregion
}
