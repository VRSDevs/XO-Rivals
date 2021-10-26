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
        _GameManager.IsWebGLMobile = CheckWebGLVersion();
        _GameManager.updateLog(
                _GameManager.IsWebGLMobile ? "mobile" : "pc"
            );
    }

    #endregion

    #region CheckMethod
    
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")] static extern bool IsMobile();
#endif
    
    /// <summary>
    /// 
    /// </summary>
    bool CheckWebGLVersion()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return IsMobile();
#endif
        _GameManager.updateLog("holi");
        return false;
    }
    
    #endregion
}
