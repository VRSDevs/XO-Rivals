using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskAdvise : Node
{


    private EnemyBT _tree;


    public TaskAdvise(EnemyBT tree)
    {
        _tree = tree;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKADVISE");

        //CAMBIO ANIMACION
        foreach (Transform t in _tree.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Enemy2D")
            {
                //Cosas que hacer
                Debug.Log(t.name);
                t.GetComponent<Animator>().SetBool("Alert", true);
                t.GetComponent<Animator>().SetBool("Normal", false);
                t.GetComponent<Animator>().SetBool("Ask", false);

            }
        }

        _tree.lastSeenPlayer = new Vector3(_tree.enemyNav.player.position.x, _tree.enemyNav.player.position.y, _tree.enemyNav.player.position.z);
        foreach (EnemyBT otherTree in _tree.otherEnemyBT)
        {
            otherTree.setAdvise(_tree.lastSeenPlayer);
        }
        

            state = NodeState.SUCCESS;
            return state;


    }



}
