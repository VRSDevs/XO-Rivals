using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundTypes {
    M,
    S
}
public class Sound : MonoBehaviour
{
    
    public AudioClip clip;
    [HideInInspector]public AudioSource source;     // Esta variable no se mostrará en el inspector

    public string name;
    public soundTypes type;

    [Range(0f,1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    [SerializeField]public bool loop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
