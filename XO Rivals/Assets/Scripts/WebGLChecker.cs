using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLChecker : MonoBehaviour
{

    #region Variables

    [SerializeField] public GameManager _GameManager;
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
            _GameManager.IsWebGLMobile = true;
            _GameManager.updateLog("MOBILE");
        }
        else //PLAYING ON PC
        {
            _GameManager.IsWebGLMobile = false;
            _GameManager.updateLog("PC");
        }






    }



    #endregion


}
