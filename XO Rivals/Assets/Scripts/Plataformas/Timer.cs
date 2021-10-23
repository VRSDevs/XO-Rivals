using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{

    public TextMeshProUGUI crono;

    private float time = 5f;
    // Start is called before the first frame update
    void Start()
    {
        crono.text = " " + time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            crono.text = " " + time.ToString("f0"); 
        }


        if (time < 1)
        {
            Debug.Log("vergalarga de atnonio rafael ruano rodriguez");
        }
    }  
}
