using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{

    // Prefabs
    [SerializeField]
    GameObject carne;
    [SerializeField]
    GameObject lechuga;
    [SerializeField]
    GameObject pan;
    [SerializeField]
    GameObject queso;

    // Variables
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;


    // Posicion inicial
    Vector3 posini = new Vector3(0,10,0);

    Quaternion quat = new Quaternion();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("generadorGlobal" ,spawnTime, spawnDelay);
    }

    public void generadorGlobal()
    {
        int rand = Random.Range(0,4);

        if (stopSpawning)
        {
            CancelInvoke("generadorGlobal");
        }

        switch (rand)
        {
            case 0:
                generarCarne();
                break;
            
            case 1:
                generarLechuga();
                break;

            case 2:
                generarPan();
                break;

            case 3:
                generarQueso();
                break;
        }

    }


    public Vector3 randPos()
    {
        posini.x = Random.Range(-5, 5);
        return posini;
    }

    
    public void generarQueso()
    {
        randPos();
        Instantiate(queso, posini, quat);
    }

    public void generarLechuga()
    {
        randPos();
        Instantiate(lechuga, posini, quat);
    }

    public void generarPan()
    {
        randPos();
        Instantiate(pan, posini, quat);
    }

    public void generarCarne()
    {
        randPos();
        Instantiate(carne, posini, quat);
    }

}
