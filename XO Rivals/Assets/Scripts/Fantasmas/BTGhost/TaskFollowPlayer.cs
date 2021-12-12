using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskFollowPlayer : Node
{

    private Transform taskTarget;
    private EnemyNavMesh _enemyNav;
    private EnemyBT _tree;


    public TaskFollowPlayer(EnemyNavMesh enemyNav, EnemyBT tree)
    {
        _enemyNav = enemyNav;
        _tree = tree;

    }




    public override NodeState Evaluate()
    {
        Debug.Log("EXECUTE TASKFOLLOWPLAYER");

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

        _tree.following = true;
        _tree.perseguirHuella = false;

        _enemyNav.target.position = _enemyNav.player.position;



        state = NodeState.RUNNING;
        return state;

    }



}
