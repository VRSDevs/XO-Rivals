using System.Collections.Generic;

//Enum of types of nodes
public enum enemyNodeType{FIRST, SELECTOR, SEQUENCE, ACTION, CONDITION}
//Delegate for actions
public delegate void Action();
//Enum for operators
public enum simOperator{EQ, GT, GTE, LT, LTE}

public abstract class enemyNode{

    //List of children
    public List<enemyNode> children;
    //Reference to parent
    public enemyNode parent;

    //Node type
    public enemyNodeType type;
    //Control for children update
    public int numSelect;
    public bool control;


    public enemyNode(enemyNode parnt){
        parent = parnt;
        children = new List<enemyNode>();
        numSelect = 0;
    }

    public void addChild(enemyNode child){
        children.Add(child);
    }

    public void addChildren(List<enemyNode> child){
        for(int i = 0; i < child.Count; i++)
            children.Add(child[i]);
    }

    public void removeChild(enemyNode child){
        children.Remove(child);
    }

    public void clearChildren(){
        children.Clear();
    }

    public abstract bool update();
    
}

public class enemyFirstNode : enemyNode{

    public enemyFirstNode() : base(null){
        type = enemyNodeType.FIRST;
    }

    //Just calls update of child
    public override bool update(){
        return children[0].update();
    }
}

public class enemySelectorNode : enemyNode{

    public enemySelectorNode(enemyNode parnt) : base(parnt){
        type = enemyNodeType.SELECTOR;
        control = false;
    }

    public override bool update(){
        //Reset value every update
        control = false;
        numSelect = 0;

        //If there are children left
        while(numSelect < children.Count){
            //Call update from children
            control = children[numSelect].update();
            numSelect++;

            if(control)
                return control;
        }
        return control;
    }
}

public class enemySequenceNode : enemyNode{

    public enemySequenceNode(enemyNode parnt) : base(parnt){
        type = enemyNodeType.SEQUENCE;
        control = true;
    }

    public override bool update(){
        //Reset value every update
        control = true;
        numSelect = 0;

        //If there are children left
        while(numSelect < children.Count){
            //Call update from children
            control = children[numSelect].update();
            numSelect++;

            if(!control)
                return control;
        }
        return control;
    }
}

public class enemyConditionNode<T> : enemyNode where T : System.IComparable {

    //Variable and comparing value
    T variable, compareValue;
    simOperator operat;

    public enemyConditionNode(enemyNode parnt, simOperator op,  T param, T val) : base(parnt){
        type = enemyNodeType.CONDITION;
        operat = op;
        variable = param;
        compareValue = val;
    }

    public override bool update(){
        //Check condition
        switch(operat){

            case simOperator.EQ:
                if(variable.CompareTo(compareValue) == 0)
                    return true;
                else
                    return false;

            case simOperator.GT:
                if(variable.CompareTo(compareValue) > 0)
                    return true;
                else
                    return false;

            case simOperator.GTE:
            if(variable.CompareTo(compareValue) >= 0)
                    return true;
                else
                    return false;

            case simOperator.LT:
            if(variable.CompareTo(compareValue) < 0)
                    return true;
                else
                    return false;

            case simOperator.LTE:
            if(variable.CompareTo(compareValue) <= 0)
                    return true;
                else
                    return false;

            default:
                return false;
        }
    }

    public void updateValue(T newVal){
        variable = newVal;
    }
}

public class enemyActionNode : enemyNode{

    //Action of the actionNode
    Action nodeAction;

    public enemyActionNode(enemyNode parnt, Action nodeAct) : base(parnt){
        type = enemyNodeType.ACTION;
        nodeAction = nodeAct;
    }

    public override bool update(){
        //Do action
        nodeAction();
        return true;
    }
}