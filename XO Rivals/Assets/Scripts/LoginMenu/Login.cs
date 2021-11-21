using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using TMPro;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

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
        
        FindObjectOfType<AudioManager>().Play("Main_menu");
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

            Log.text = "Connecting...";
            
            if (_gameManager.OnConnectToServer())
            {
                Log.text = "Connected.";
                
                _gameManager.SetPhotonNick(username);
                
                GameObject myPlayer = new GameObject();
                PlayerInfo playerInfo = myPlayer.AddComponent<PlayerInfo>();
                DontDestroyOnLoad(myPlayer);
                playerInfo.name = "PlayerObject";
                playerInfo.Name = username;
                playerInfo.ID = Authenticator.playFabPlayerIdCache;
                
                //Get client data from PlayFab
                PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest() {
                    PlayFabId = playerInfo.ID,
                    Keys = null
                }, result => {
                    Debug.Log("Got user data:");
                    if (result.Data != null){
                        //Get lifes
                        if(result.Data.ContainsKey("Lifes")){
                            playerInfo.Lives = int.Parse(result.Data["Lifes"].Value);
                            playerInfo.Lives = 3;
                            Debug.Log("Successfully got player lifes");
                        }else{
                            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                                Data = new Dictionary<string, string>() {
                                    {"Lifes", "5"}
                                }
                            },
                            result => Debug.Log("Successfully updated user lifes"),
                            error => {
                                Debug.Log("Got error setting user lifes");
                            });
                        }

                        //Get level
                        if(result.Data.ContainsKey("Level")){
                            playerInfo.Level = float.Parse(result.Data["Level"].Value);
                            playerInfo.Level += 3.82f;
                            Debug.Log("Successfully got player level");
                        }else{
                            PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                                Data = new Dictionary<string, string>() {
                                    {"Level", "0.0"}
                                }
                            },
                            result => Debug.Log("Successfully updated user level"),
                            error => {
                                Debug.Log("Got error setting user level");
                            });
                        }

                        //Get moment of life lost(if exists)
                        if(result.Data.ContainsKey("Life Lost") && result.Data["Life Lost"].Value != "" && result.Data["Life Lost"].Value != null){
                            playerInfo.LostLifeTime = DateTime.ParseExact(result.Data["Life Lost"].Value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            Debug.Log("Successfully got player life lost moment");
                        }  
                        playerInfo.LostLifeTime = System.DateTime.Now;               
                    }else{
                        //Setup all information
                        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest() {
                            Data = new Dictionary<string, string>() {
                                {"Lifes", "5"},
                                {"Level", "0"}
                            }
                        },
                        result => Debug.Log("Successfully updated user information"),
                        error => {
                            Debug.Log("Got error setting user information");
                        });
                    }
                }, (error) => {
                    Debug.Log("Got error retrieving user data:");
                });
                
            }
            else
            {
                Log.text = "Oops! Something went wrong.";
                IsConnecting = false;
            }
        }
        catch (LoginFailedException e)
        {
            switch (e.ErrorCode)
            {
                case PlayFabErrorCode.UsernameNotAvailable:
                    Log.text = "Username not available.";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    Log.text = "Account not found.";
                    break;
                case PlayFabErrorCode.InvalidUsernameOrPassword:
                    Log.text = "Invalid username or password.";
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
            Log.text = "Incorrect length. Minimum " + MIN_CHARS + " chars.";
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
