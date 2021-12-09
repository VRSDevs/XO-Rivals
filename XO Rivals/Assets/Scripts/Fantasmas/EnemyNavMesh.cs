using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public List<Transform> movePositionTransform; //Lista de targets
    public Transform target; //Hacia donde se mueve
    public Transform player;
    //public bool persiguiendo = false; //Estoy persiguiendo un objetivo?
    int random = 0;
    public bool move = false; //Debe moverse?


    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        /*
        int random = Random.Range(0, movePositionTransform.Count - 1);
        target = movePositionTransform[random];
        */
       
    }

    // Update is called once per frame
    void Update()
    {


            navMeshAgent.destination = target.position;

      
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
    
            if (other.gameObject.name == target.name)
            {
                random = Random.Range(0, movePositionTransform.Count - 1);
                target = movePositionTransform[random];
            }
        */


    }




}
