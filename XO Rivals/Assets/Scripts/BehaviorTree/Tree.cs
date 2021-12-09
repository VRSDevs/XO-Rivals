using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {

        private Node _root = null;
        protected float waitTime = -1;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {

            waitTime -= Time.deltaTime;

            if (waitTime < 0)//YA NO ESPERAMOS
            {

                if (_root != null)
                    _root.Evaluate();
            }
        

        }

        protected abstract Node SetupTree();

    }

}
