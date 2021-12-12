using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptHuella : MonoBehaviour
{

    public GameObject sigHuella;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(deleteHuella());


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator deleteHuella()
    {
        yield return new WaitForSeconds(15f);

        Destroy(this.gameObject);

    }

}
