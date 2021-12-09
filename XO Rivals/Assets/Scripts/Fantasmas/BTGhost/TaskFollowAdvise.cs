using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskFollowAdvise : Node
{


    private EnemyBT _tree;
    private EnemyNavMesh _enemyNav;


    public TaskFollowAdvise(EnemyBT tree, EnemyNavMesh enemyNav)
    {
        _tree = tree;
        _enemyNav = enemyNav;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKFOLLOWADVISE");

        _tree.following = false;

        _enemyNav.target.position = _tree.adviseVector;
        if (Vector3.Distance(_enemyNav.transform.position, _tree.adviseVector) < 2f) //HASTA QUE NO LLEGUE A LA POSICION INDICADA NO PARA
        {
            _tree.setAdviseFalse();

        }

        state = NodeState.RUNNING;
        return state;

    }


}
