using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsAnimation : MonoBehaviour
{

    [SerializeField]
    private Animator animPlayerStats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPlayerStats()
    {
        animPlayerStats.SetBool("close", true);
    }
    public void ClosePlayerStats()
    {
        animPlayerStats.SetBool("close", false);
    }
}
