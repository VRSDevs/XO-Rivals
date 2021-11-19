using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
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
    #region Vars
    
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
    [SerializeField] public TMP_InputField UsernameInput;
    /// <summary>
    /// Referencia (en Registro) al InputField de la constraseña
    /// </summary>
    [SerializeField] public TMP_InputField PasswordInput;
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

    ////////////////// TECLADO EN MÓVILES //////////////////
    TouchScreenKeyboard keyboard;


    #endregion

    #region UnityCB

    private void Start()
    {
        // Limitación de caracteres máximos de los inputs
        UsernameInput.characterLimit = MAX_CHARS;
        PasswordInput.characterLimit = MAX_CHARS;

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

            string username = UsernameInput.text, password = PasswordInput.text;
            
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
        Debug.Log("Modo: " + Mode);
        Log.text = "Validando credenciales...";
        Authenticator.AuthWithPlayfab(username, password, Mode);
    }

    /// <summary>
    /// Método ejecutado como corutina para establecer conexión con el servidor
    /// </summary>
    /// <param name="username">Nombre de usuario</param>
    /// <param name="password">Contraseña</param>
    /// <param name="mode">Modo de inicio de sesión</param>
    /// <returns></returns>
    /// <exception cref="LoginFailedException">Excepción producida por fallo al iniciar sesión</exception>
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
            
            Authenticator.Reset();

            Log.text = "Conectando...";
            
            if (_gameManager.OnConnectToServer())
            {
                Log.text = "Conectado.";
                _gameManager.SetPhotonNick(username);
                GameObject myPlayer = new GameObject();
                PlayerInfo playerInfo = myPlayer.AddComponent<PlayerInfo>();
                DontDestroyOnLoad(myPlayer);
                playerInfo.name = "PlayerObject";
                playerInfo.Name = username;
                //myPlayer.Id = get ID from server //innecesario
                //myPlayer.Elo = get ELO from server
            }
            else
            {
                Log.text = "Error al conectar.";
                IsConnecting = false;
            }
        }
        catch (LoginFailedException e)
        {
            switch (e.ErrorCode)
            {
                case PlayFabErrorCode.UsernameNotAvailable:
                    Log.text = "Nombre de usuario no disponible.";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    Log.text = "Cuenta no encontrada.";
                    break;
                case PlayFabErrorCode.InvalidUsernameOrPassword:
                    Log.text = "Nombre de usuario o contraseña inválido.";
                    break;
            }

            IsConnecting = false;

            Authenticator = new PlayFabAuthenticator();

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
        // Eliminación de espacios en la cadena de caracteres
        UsernameInput.text = UsernameInput.text.Trim();
        PasswordInput.text = PasswordInput.text.Trim();
                
        if (UsernameInput.text.Length < MIN_CHARS || PasswordInput.text.Length < MIN_CHARS)
        {
            Log.text = "Longitud no correcta, mínimo " + MIN_CHARS;
            return false;
        }

        return true;
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
        switch (PasswordInput.contentType)
        {
            case TMP_InputField.ContentType.Standard:
                PasswordInput.contentType = TMP_InputField.ContentType.Password;
                break;
            case TMP_InputField.ContentType.Password:
                PasswordInput.contentType = TMP_InputField.ContentType.Standard;
                break;
        }
        
        // Método para forzar la actualización de muestra del input
        PasswordInput.ForceLabelUpdate();
    }

    /// <summary>
    /// Método para limpiar los campos de los inputs
    /// </summary>
    public void ClearFields()
    {
        UsernameInput.text = "";
        PasswordInput.text = "";
        
        // Método para forzar la actualización de muestra del input
        UsernameInput.ForceLabelUpdate();
        PasswordInput.ForceLabelUpdate();
    }

    public void ShowKeyboard()
    {
        
        
        keyboard = TouchScreenKeyboard.Open("");
        keyboard.active = true;
        
    }

    public void HideKeyboard()
    {

        keyboard.active = false;
        
    }
    

    #endregion
}
