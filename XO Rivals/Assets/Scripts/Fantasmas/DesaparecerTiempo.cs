using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaparecerTiempo : MonoBehaviour
{
    public bool[] visible;

    public int porcentajeInvisible = 20; //cada segundo un X% de probabilidades

    public List<GameObject> muros;

    // Start is called before the first frame update
    void Start()
    {


        visible = new bool[muros.Count];

        StartCoroutine(segundoPosibleInvisible());
    }

    // Update is called once per frame
    void Update()
    {
        //if (visible == false && transicion == true) 
        //{
        //    if (gameObject.GetComponent<Renderer>().material.color.a == 0) // SI YA ES INVISIBLE ACABA TRANSICION
        //    {
        //        transicion = false;
        //    }
        //    else
        //    {
        //               }
        //}
        //else if (visible ==true && transicion == true) 
        //{
        //    if (gameObject.GetComponent<Renderer>().material.color.a == 254) // SI YA ES vISIBLE ACABA TRANSICION
        //    {
        //        transicion = false;
        //    }
        //    else
        //    {
        //        Debug.Log("LOCO");
        //        gameObject.GetComponent<Renderer>().material.color = new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, gameObject.GetComponent<Renderer>().material.color.a + 1f);
        //    }
        //}




    }
    IEnumerator segundoPosibleInvisible()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < muros.Count; i++)
        {//POR CADA MURO

            int rand = Random.Range(1, 100);
            if (rand <= porcentajeInvisible)//SI TRANSICIONA
            {


                if (visible[i])
                {
                    muros[i].SetActive(false);

                }
                else
                {
                    muros[i].SetActive(true);
                }


                visible[i] = !visible[i];

            }
        }

        StartCoroutine(segundoPosibleInvisible());

    }
   
            
        



    }




