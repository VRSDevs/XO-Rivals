using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Login : GameManager
{
    #region Variables
    
    /// <summary>
    /// Mínimo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MIN_CHARS = 3;
    /// <summary>
    /// Máximo número de caracteres para nombre de usuario y contraseña
    /// </summary>
    private const int MAX_CHARS = 24;

    [SerializeField] public TMP_InputField Username;
    [SerializeField] public TMP_InputField Password;
    [SerializeField] public TMP_Text LoginInfo;

    #endregion
}
