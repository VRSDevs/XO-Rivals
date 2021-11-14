using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonCred : MonoBehaviour
{
    public bool inside = false;
    [SerializeField] public GameObject MainMenuObject;
    [SerializeField] public GameObject CredMenuObject;
    public CambioCreditos cambiocreditos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickBack()
    {

        if (inside == false)
        {
            MainMenuObject.SetActive(true);
            CredMenuObject.SetActive(false);
        }else
        {
            cambiocreditos.UnExplainButton();

            inside = false;
        }




    }

    public void clickInside()
    {
        inside = true;
    }


}
