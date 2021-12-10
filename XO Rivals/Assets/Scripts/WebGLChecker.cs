using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class WebGLChecker : MonoBehaviour
{
    private AudioSource managerBoton;
    #region Variables

    /// <summary>
    /// Referencia al GameManager
    /// </summary>
    [SerializeField] public GameManager _gameManager;
    /// <summary>
    /// ¿Es ejecutado en móvil?
    /// </summary>
    public bool isMobile;

    #endregion

    #region CheckMethod

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    static extern bool IsMobile();
#endif

    void CheckIfMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();

#endif
    }

    #endregion
    
    #region UnityCB

    void Start()
    {
        CheckIfMobile();
        if (isMobile) //PLAYING ON MOBILE
        {
            _gameManager.IsWebGLMobile = true;
            //_gameManager.updateLog("MOBILE");
        }
        else //PLAYING ON PC
        {
            _gameManager.IsWebGLMobile = false;
            //_gameManager.updateLog("PC");
        }
    }
    #endregion
}
