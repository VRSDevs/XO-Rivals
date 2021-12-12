using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PlayFab;
using TMPro;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Modo de inicio de sesión activo
///     None - Ninguno
///     Register - Registrarse
///     Login - Iniciar sesión
/// </summary>
[Serializable]
public enum LoginMode
{
    None,
    Register,
    Login
}

public class Login : MonoBehaviour
{
    #region Vars
    
    ////////////////// REFERENCIAS A CLASES //////////////////
    /// <summary>
    /// Referencia a la clase GameManager
    /// </summary>
    private GameManager _gameManager;
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
    private bool _isConnecting;
    /// <summary>
    /// 
    /// </summary>
    private PlayerInfo _playerInfo;
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    private string _username;
    /// <summary>
    /// Contraseña
    /// </summary>
    private string _password;

    
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
        FindObjectOfType<AudioManager>().StopAllSongs();

        _gameManager = FindObjectOfType<GameManager>();
        
        // Generación objeto de PlayerInfo
        GameObject myPlayer = new GameObject();
        _playerInfo = myPlayer.AddComponent<PlayerInfo>();
        DontDestroyOnLoad(myPlayer);
        _playerInfo.name = "PlayerObject";
        
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
        _username = UsernameInput.text;
        _password = PasswordInput.text;
        
        if (!_isConnecting && ValidateInputs())
        {
            _isConnecting = true;
                
            OnAuthentication(_username, _password);
            StartCoroutine(EstablishConnection());
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
        Log.text = "Validating...";
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
    private IEnumerator EstablishConnection()
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

            Log.text = "Connecting to server...";
            if (_gameManager.OnConnectToServer())
            {
                StartCoroutine(OnEstablishedConnection());
            }
            else
            {
                Log.text = "Oops! Something went wrong.";
                _isConnecting = false;
                Authenticator = gameObject.AddComponent<PlayFabAuthenticator>();
            }
        }
        catch (LoginFailedException e)
        {
            switch (e.ErrorCode)
            {
                case PlayFabErrorCode.UsernameNotAvailable:
                    Log.text = "Username not available. Already exists!";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    Log.text = "Account not found.";
                    break;
                case PlayFabErrorCode.InvalidUsernameOrPassword:
                    Log.text = "Invalid username or password.";
                    break;
            }

            _isConnecting = false;
            Authenticator = gameObject.AddComponent<PlayFabAuthenticator>();

            Debug.Log("[SISTEMA]: " + e.Message + ". (" + e.ErrorCode + ")");
        }
    }

    /// <summary>
    /// Corutina ejecutada tras establecer conexión con el servidor
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnEstablishedConnection()
    {
        yield return new WaitUntil(_gameManager.GetConnected);
        
        _gameManager.SetPhotonNick(_username);
        
        _playerInfo.UserID = PhotonNetwork.LocalPlayer.UserId;
        _playerInfo.Name = _username;

        _gameManager.GetCloudData(DataType.Login);
        Log.text = "Getting data...";

        StartCoroutine(OnOnlineChecked()); 
    }

    /// <summary>
    /// Corutina ejecutada tras comprobar si el usuario está conectado
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnOnlineChecked()
    {
        yield return new WaitUntil(_gameManager.GetCheckedOnline);

        try
        {
            if (_gameManager.GetOnlineAuth().Failed)
            {
                throw new LoginFailedException(_gameManager.GetOnlineAuth().Message)
                {
                    ErrorCode = _gameManager.GetOnlineAuth().ErrorCode
                };
            }
            
            _gameManager.ResetOnlineAuth();
            StartCoroutine(OnGetPlayerData()); 
        }
        catch (LoginFailedException e)
        {
            switch (e.ErrorCode)
            {
                case PlayFabErrorCode.ConnectionError:
                    Log.text = "This user is already connected.";
                    break;
            }

            _gameManager.ResetOnlineAuth();

            _gameManager.OnDisconnectToServer();
            _isConnecting = false;

            Debug.Log("[SISTEMA]: " + e.Message + ". (" + e.ErrorCode + ")");
        }
    }

    /// <summary>
    /// Corutina ejecutada tras obtener datos de la nube
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnGetPlayerData()
    {
        yield return new WaitUntil(_gameManager.GetSynchronizeStatus);

        Dictionary<string, string> data = _gameManager.GetDataDictionary();

        switch (int.Parse(data["ResultCode"]))
        {
            case 1:

                data[DataType.Online.GetString()] = "true";
                _playerInfo.Online = bool.Parse(data[DataType.Online.GetString()]);
                _playerInfo.Lives = int.Parse(data[DataType.Lives.GetString()]);
                _playerInfo.Level = float.Parse(data[DataType.Level.GetString()]);
                _playerInfo.LostLifeTime = DateTime.ParseExact(data[DataType.LifeLost.GetString()],
                    "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                
                _gameManager.UpdateCloudData(data, DataType.Login);
                            
                break;
            case 2:

                _playerInfo.Online = true;
                _playerInfo.Lives = 3;
                _playerInfo.Level = 0.0f;

                _gameManager.UpdateCloudData(new Dictionary<string, string>()
                {
                    {DataType.Online.GetString(), _playerInfo.Online.ToString()},
                    {DataType.Lives.GetString(), _playerInfo.Lives.ToString()},
                    {DataType.Level.GetString(), _playerInfo.Level.ToString(CultureInfo.InvariantCulture)}
                },
                    DataType.Login);
                        
                break;
            case 3:
                Debug.Log("Got error retrieving user data:");
                        
                break;
            default:
                break;
        }

        Log.text = "Connecting to lobby...";
        _gameManager.OnConnectToLobby();
    }

    #endregion

    #region CheckMethods

    /// <summary>
    /// Método para validar la longitud de los inputs
    /// </summary>
    /// <returns>¿Inputs válidos?</returns>
    private bool ValidateInputs()
    {
        // Sin espacios
        if (_username.Contains(" ") || _password.Contains(" "))
        {
            Log.text = "No whitespaces allowed";

            _isConnecting = false;
            return false;
        }

        // Contraseña con mínimo de caracteres
        if (_password.Length < MIN_CHARS)
        {
            Log.text = "Incorrect length. Password has a minimum of " + MIN_CHARS + " chars.";
            
            _isConnecting = false;
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
                Mode = LoginMode.None;
                break;
            case 1:
                Mode = LoginMode.Register;
                break;
            case 2:
                Mode = LoginMode.Login;
                break;
        }
    }

    public void SelectButton()
    { 
        FindObjectOfType<AudioManager>().Play("SelecctionButton1");
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
