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

    public GameObject prefabBala;
    public GameObject bala;

    void Start()
    {
        //Debug.Log(Vector3.Distance(player.transform.position,this.transform.position));
        StartCoroutine(creaBala());
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

       




    }


    public void playerDetected(bool detected)
    {

        veoPlayer = detected;

        Debug.Log(veoPlayer);

    }

    IEnumerator creaBala()
    {
        yield return new WaitForSeconds(0.7f);

        //INSTANCIAR PREFAB
        bala = Instantiate(prefabBala, this.transform.position, Quaternion.identity);

        //CODE
        StartCoroutine(creaBala());
    }


    //void OnCollisionEnter2D(Collision2D collision)
    //{


    //    Debug.Log("AAA");



    //}


}
