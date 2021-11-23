using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{

    [SerializeField] public Button PlayBut;
    [SerializeField] public Button ShopBut;
    [SerializeField] public Button CredBut;
    [SerializeField] public Button SettBut;

    public void ResizeBut()
    {

        PlayBut.gameObject.transform.localScale += new Vector3(1, 1, 1);
        ShopBut.gameObject.transform.localScale += new Vector3(1, 1, 1);
        CredBut.gameObject.transform.localScale += new Vector3(1, 1, 1);
        SettBut.gameObject.transform.localScale += new Vector3(1, 1, 1);

    }
}
