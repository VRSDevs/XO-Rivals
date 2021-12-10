using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskPerseguirHuella : Node
{


    private EnemyBT _tree;
    private EnemyNavMesh _enemyNav;


    public TaskPerseguirHuella(EnemyBT tree, EnemyNavMesh enemyNav)
    {
        _tree = tree;
        _enemyNav = enemyNav;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKPERSEGUIRHUELLA");

        _tree.following = false;

        ScriptHuella huella = _tree.lastHuella.GetComponent<ScriptHuella>();

        _enemyNav.target.position = huella.sigHuella.transform.position;
      

        state = NodeState.RUNNING;
        return state;

    }
}
