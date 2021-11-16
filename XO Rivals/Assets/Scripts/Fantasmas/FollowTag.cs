using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTag : MonoBehaviour
{
    public GameObject player;
    public string tag;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag(tag);
    }

    // Update is called once per frame
    void Update()
    {
      
        this.gameObject.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y,  player.transform.position.z );


    }
}
