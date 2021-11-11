using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaDetectScript : MonoBehaviour
{
    public GameObject detectPlayer;
    DetectPlayer detectScript;

    // Start is called before the first frame update
    void Start()
    {
        
        detectPlayer = GameObject.FindWithTag("Enemy1");
        detectScript = detectPlayer.GetComponent<DetectPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
