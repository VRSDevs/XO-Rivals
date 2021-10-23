using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{

    public TextMeshProUGUI crono;

    private float time = 10f;
    // Start is called before the first frame update
    void Start()
    {
        crono.text = " " + time;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        crono.text = " " + time.ToString("f0");
    }  
}
