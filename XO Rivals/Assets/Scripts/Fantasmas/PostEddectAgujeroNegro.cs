using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEddectAgujeroNegro : MonoBehaviour
{
    public Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        Graphics.Blit(source,destination,mat);

    }





    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void SetPosicionPlayer(float x,float y)
    {
        mat.SetVector("_PosPlayer", new Vector3(x,y,0));

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
