using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public List<Transform> movePositionTransform;
    public Transform target;
    


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
       

        if (other.gameObject.name == target.name)
        {
            int random = Random.Range(0, movePositionTransform.Count - 1);
            target = movePositionTransform[random];
        }

       
    }
    private void OnCollisionEnter(Collision collision)
    {
      


        Debug.Log(collision.gameObject.name);
    }



}
