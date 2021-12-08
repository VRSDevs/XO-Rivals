using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform taskTarget;
    private bool newTarget = true; //CAMBIAMOS DE OBJETIVO
    private EnemyNavMesh _enemyNav;
    private EnemyBT _tree;

    public TaskPatrol( EnemyNavMesh enemyNav, EnemyBT tree)
    {
        _enemyNav = enemyNav;
        _tree = tree;
    }


      
  
    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKPATROL");

        _tree.following = false;

        if (newTarget)
        {
            newTarget = false;
            //SELECCIONAMOS UN TARGET AL AZAR
            int random = Random.Range(0, _enemyNav.movePositionTransform.Count - 1);
            taskTarget = _enemyNav.movePositionTransform[random];
        }

        _enemyNav.target = taskTarget;

        //SI LLEGAMOS A UN PUNTO DE PATRULLA DEBERIAMOS IR AL SIGUIENTE
        if (Vector3.Distance(_enemyNav.transform.position, taskTarget.position) < 0.1f)
        {
            newTarget = true; //CAMBIAMOS DE TARGET
        }


        state = NodeState.RUNNING;
        return state;

    }

}
