using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPatrolLuc : Node
{

    private Transform taskTarget;
    private bool newTarget = true; //CAMBIAMOS DE OBJETIVO
    private EnemyNavMesh _enemyNav;

    public TaskPatrolLuc(EnemyNavMesh enemyNav)
    {
        _enemyNav = enemyNav;
    }



    public override NodeState Evaluate()
    {



        if (newTarget)
        {
            newTarget = false;
            //SELECCIONAMOS UN TARGET AL AZAR
            int random = Random.Range(0, _enemyNav.movePositionTransform.Count - 1);
            taskTarget = _enemyNav.movePositionTransform[random];
        }

        _enemyNav.target.position = taskTarget.position;

        //SI LLEGAMOS A UN PUNTO DE PATRULLA DEBERIAMOS IR AL SIGUIENTE
        if (Vector3.Distance(_enemyNav.transform.position, taskTarget.position) <1f)
        {
            newTarget = true; //CAMBIAMOS DE TARGET
        }


        state = NodeState.RUNNING;
        return state;

    }


}
