using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyBT : BehaviorTree.Tree
{
    public EnemyNavMesh enemyNav;
    public DetectPlayer detectPlayer;
    public bool advised = false;
    public Vector3 adviseVector;

    public List<EnemyBT> otherEnemyBT;

    public bool following = false;

    public Vector3 lastSeenPlayer;

    public GameObject lastHuella;
    public bool perseguirHuella = false;

    protected override Node SetupTree()
    {


        Node root = new Selector(new List<Node>
        {
          

            new Sequence(new List<Node>                      // PRIMERO COMPROBAMOS SI VEMOS AL ENEMIGO
            {
                //COMPROBACIONES PARA SABER SI LE "VEMOS" 
                new Selector(new List<Node>         
                {


                    new Sequence(new List<Node>
                    {
                        new CheckLinterna(detectPlayer),
                        new CheckNoObstacle(detectPlayer),
                    }),

                    new Sequence(new List<Node>                     
                    {
                        new CheckFollowingPlayer(this),
                        new CheckRadio(detectPlayer),
                    }),


                }),

                 new TaskFollowPlayer(enemyNav,this),
                 new TaskAdvise(this),


            }),

             new Sequence(new List<Node>                      //SEGUNDO COMPROBAMOS SI HEMOS SIDO AVISADOS
             {
                 new CheckGetAdvise(this),
                 new TaskFollowAdvise(this,enemyNav),
             }),

               new Sequence(new List<Node>                      //SEGUNDO COMPROBAMOS SI HEMOS SIDO AVISADOS
             {
                 new CheckPerseguirHuella(this),
                 new TaskPerseguirHuella(this,enemyNav),
             }),

            new TaskPatrol(enemyNav,this),                   //TERCERO DECIDIMOS PATRULLAR(Ultima opcion)
      

        });

        return root;

    }

    public void wait(int seconds)
    {
        waitTime = 1;
    }


    public void setAdvise(Vector3 vectorA)
    {
        adviseVector = vectorA;
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
