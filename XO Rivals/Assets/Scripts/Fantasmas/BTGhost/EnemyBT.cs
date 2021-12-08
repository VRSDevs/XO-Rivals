using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyBT : BehaviorTree.Tree
{
    public EnemyNavMesh enemyNav;
    public DetectPlayer detectPlayer;
    private bool advised = false;
    private Transform adviseTransform;

    protected override Node SetupTree()
    {


        Node root = new Selector(new List<Node>
        {
            //new Sequence(new List<Node>
            //{
            //    new CheckEnemyInAttackRange(transform),
            //    new TaskAttack(transform),
            //}),
            new Sequence(new List<Node>
            {
                new CheckLinterna(detectPlayer),
                new TaskFollowPlayer(enemyNav),
            }),
            new TaskPatrol(enemyNav),
      

        });

        return root;

    }

    public void wait(int seconds)
    {
        waitTime = 1;
    }


    public void setAdvise(Transform transformA)
    {
        adviseTransform = transformA;
        advised = true;
    }
    public void setAdviseFalse()
    {
        advised = false;
    }
    public bool getAdvise()
    {
        return advised;
    }

}
