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

        //CAMBIO ANIMACION
        foreach (Transform t in _tree.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Enemy2D")
            {
                //Cosas que hacer
                Debug.Log(t.name);
                t.GetComponent<Animator>().SetBool("Ask", true);
                t.GetComponent<Animator>().SetBool("Normal", false);
                t.GetComponent<Animator>().SetBool("Alert", false);

            }
        }


        _tree.following = false;
        _tree.perseguirHuella = false;

        _enemyNav.target.position = _tree.adviseVector;
        if (Vector3.Distance(_enemyNav.transform.position, _tree.adviseVector) < 2f) //HASTA QUE NO LLEGUE A LA POSICION INDICADA NO PARA
        {
            _tree.setAdviseFalse();

        }

        state = NodeState.RUNNING;
        return state;

    }


}
