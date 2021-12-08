using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskWait : Node
{

    private EnemyBT _tree;
    private int _waitTime;


    public TaskWait(EnemyBT tree, int waitTime)
    {
        _tree = tree;
        _waitTime = waitTime;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKWAIT");

        _tree.wait(_waitTime);

        state = NodeState.SUCCESS;
        return state;


    }

}
