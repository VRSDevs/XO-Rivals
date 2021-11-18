using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public List<Transform> movePositionTransform;
    public Transform target;
    public Transform player;
    public bool persiguiendo = false;
    int random = 0;


    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        int random = Random.Range(0, movePositionTransform.Count - 1);
        target = movePositionTransform[random];
    }

    // Update is called once per frame
    void Update()
    {

        
        navMeshAgent.destination = target.position;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (persiguiendo == false)
        {

            if (other.gameObject.name == target.name)
            {
                random = Random.Range(0, movePositionTransform.Count - 1);
                target = movePositionTransform[random];
            }



        }



    }

  public void playerDetectedLinternaNav(bool detected)
    {
        persiguiendo = detected;


        if (detected)
        {
            target = player;
        }
        else
        {
            
            target = movePositionTransform[random];
        }





    }



}
