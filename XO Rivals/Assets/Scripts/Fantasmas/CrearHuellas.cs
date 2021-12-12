using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearHuellas : MonoBehaviour
{

    public GameObject prefabHuella;
    public GameObject huella;
    public GameObject huellaAnt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(creaHuella());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator creaHuella()
    {
        yield return new WaitForSeconds(0.5f);

        //INSTANCIAR PREFAB
        huella = Instantiate(prefabHuella, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);

        if (huellaAnt != null)
        {
            huellaAnt.GetComponent<ScriptHuella>().sigHuella = huella;
        }

        huellaAnt = huella;


        //CODE
        StartCoroutine(creaHuella());
    }

}
