using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWinTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        int randomWin = Random.Range(0,3);

        if (randomWin == 0) //SALIDA 0
        {
           
            GameObject.FindGameObjectsWithTag("Win2")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win3")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win4")[0].SetActive(false);

            GameObject.Find("SalidaCerrada1").SetActive(false);

        }

        if (randomWin == 1) //SALIDA 0
        {

            GameObject.FindGameObjectsWithTag("Win1")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win3")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win4")[0].SetActive(false);

            GameObject.Find("SalidaCerrada2").SetActive(false);

        }

        if (randomWin == 2) //SALIDA 0
        {
            GameObject.FindGameObjectsWithTag("Win1")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win2")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win4")[0].SetActive(false);

            GameObject.Find("SalidaCerrada3").SetActive(false);

        }

        if (randomWin == 3) //SALIDA 0
        {
            GameObject.FindGameObjectsWithTag("Win1")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win2")[0].SetActive(false);
            GameObject.FindGameObjectsWithTag("Win3")[0].SetActive(false);

            GameObject.Find("SalidaCerrada4").SetActive(false);

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
