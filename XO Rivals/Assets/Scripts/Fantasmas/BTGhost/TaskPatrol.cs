using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform taskTarget;
    private bool newTarget = true; //CAMBIAMOS DE OBJETIVO
    private EnemyNavMesh _enemyNav;

    public TaskPatrol( EnemyNavMesh enemyNav)
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

        _enemyNav.target = taskTarget;  



       state = NodeState.RUNNING;
        return state;

    }

}
