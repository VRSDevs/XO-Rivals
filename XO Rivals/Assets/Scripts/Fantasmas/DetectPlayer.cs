using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public float distanciaVision = 4;
    public bool veoPlayer = false;
    public List<GameObject> obstaculos;

    void Start()
    {
        //Debug.Log(Vector3.Distance(player.transform.position,this.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < distanciaVision)
        {
            veoPlayer = true;
        }
        else
        {
            veoPlayer = false;
        }


        



        Debug.Log(veoPlayer);

    }
}
