using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform movePositionTransform;


    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

        navMeshAgent.destination = movePositionTransform.position;
        
    }
}
