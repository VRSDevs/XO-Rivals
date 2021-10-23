using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{

    public TextMeshProUGUI crono;
    public Player ScriptPlayer;
    public bool lost = false;

    private float time = 3f;
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


        if (time < 0)
        {
            if (ScriptPlayer.Victory == false)
            {
                ScriptPlayer.textValue = " Game Over";
                lost = true;
                ScriptPlayer.OnDisable();

            }

        }
    }  
}
