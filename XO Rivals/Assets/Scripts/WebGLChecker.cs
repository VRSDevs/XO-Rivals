using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLChecker : MonoBehaviour
{

    #region Variables

    [SerializeField] public GameManager _GameManager;

    #endregion
    
    #region UnityCB

    void Start()
    {
        CheckWebGLVersion();
    }

    #endregion

    #region CheckMethod
    
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")] static extern bool IsMobile();
#endif
    
    /// <summary>
    /// 
    /// </summary>
    void CheckWebGLVersion()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        _GameManager.IsWebGLMobile = IsMobile();
#endif
    }


    #endregion
}
