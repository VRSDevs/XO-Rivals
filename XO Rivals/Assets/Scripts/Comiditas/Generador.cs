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
    [SerializeField]
    GameObject panArriba;

    // Variables
    public bool startedSpawning = false;
    public bool stopSpawning = true;
    public float spawnTime;
    public float spawnDelay;

    // Control de spawn
    private bool cheese = false;
    private bool bread = false;
    private bool lettuce = false;
    private bool meat = false;
    private bool breadUp = false;

    // Posicion inicial
    Vector3 posini = new Vector3(0,10,0);

    Quaternion quat = new Quaternion();


    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("generadorGlobal" ,spawnTime, spawnDelay);
    }

    private void Update()
    {      

        if (!startedSpawning)
        {
            startedSpawning = true;
            InvokeRepeating("generadorGlobal", spawnTime, spawnDelay);
        }
    }

    public void ActivateGenerator()
    {
        startedSpawning = false;
    }

    public void generadorGlobal()
    {
        int rand = Random.Range(0,5);
        int probability = Random.Range(0,100);

        // Mandar panArriba
        if (!breadUp)
        {
            if (probability > 80)
            {
                rand = 4;
            }
        }

        // Mandar lechuga
        if (!lettuce && !breadUp)
        {
            if (probability > 60)
            {
                rand = 1;
            }
        }


        // Mandar carne
        if (!lettuce && !meat && !breadUp)
        {
            if (probability > 40)
            {
                rand = 0;
            }
        }

        // Mandar queso
        if (!cheese && !lettuce && !meat && !breadUp)
        {
            if (probability > 25)
            {
                rand = 3;
            }  
        }

        // Mandar pan abajo
        if (!cheese && !bread && !lettuce && !meat && !breadUp)
        {
            if (probability > 10)
            {
                rand = 2;
            }
        }

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
            case 4:
                generarPanArriba();
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
        cheese = true;
        Instantiate(queso, posini, quat);
    }

    public void generarLechuga()
    {
        randPos();
        lettuce = true;
        Instantiate(lechuga, posini, quat);
    }

    public void generarPan()
    {
        randPos();
        bread = true;
        Instantiate(pan, posini, quat);
    }

    public void generarPanArriba()
    {
        randPos();
        breadUp = true;
        Instantiate(panArriba, posini, quat);
    }

    public void generarCarne()
    {
        randPos();
        meat = true;
        Instantiate(carne, posini, quat);
    }

}
