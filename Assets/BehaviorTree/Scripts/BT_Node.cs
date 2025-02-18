using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BT_Node
{
    public enum Status { Success, Failure, Running }

    public string name;

    public List<BT_Node> children = new();
    protected int currentchild;

    public BT_Node(string name = "Node") 
    {
        this.name = name;
    }

    public void AddChild(BT_Node child) 
    {
        children.Add(child);
    }

    public virtual Status Process() 
    {
       return children[currentchild].Process();
    } 

    public virtual void Reset() 
    {
        currentchild = 0;
        foreach (BT_Node child in children) 
        {
            child.Reset();
        }
    }
}
public class Selector : BT_Node
{
    public Selector(string name) : base(name) { }

    public override Status Process()
    {
        if (currentchild < children.Count)
        {
            BT_Node.Status isSuccess = children[currentchild].Process();
            switch (isSuccess)
            {

                case Status.Success:
                    Reset();
                    return Status.Success;
                case Status.Running:
                    return Status.Running;
                default:
                    currentchild++;
                    return Status.Running;

            }
        }

        Reset();
        return Status.Failure;
    }
}
public class Leaf : BT_Node 
{
    ITask strategy;

    public Leaf(string name, ITask strategy) : base(name)
    {
        this.strategy = strategy;
    }

    public override Status Process() 
    {
        return strategy.Process();
    } 

    public override void Reset()
    {
        strategy.Reset();
    }
}
public class BehaviorTree : BT_Node 
{
    public BehaviorTree(string name) : base(name) { }

    public override Status Process()
    {
        while (currentchild<children.Count) 
        {
            var status = children[currentchild].Process();
            if (status !=Status.Success)
            {
                return status;
            }
            currentchild++;
        }
        return Status.Success;
    }
}

public class Sequence : BT_Node 
{
    public Sequence(string name) : base(name) { }

    public override Status Process()
    {
        if (currentchild<children.Count)
        {
            switch (children[currentchild].Process())
            {
                case Status.Running:
                    return Status.Running;

                case Status.Failure:
                    Reset();
                    return Status.Failure;

                default:

                    currentchild++;
                    if (currentchild==children.Count)
                    {
                        return Status.Success;
                    }
                    else
                    {
                        return Status.Failure;
                    }
            }
        }

        Reset();
        return Status.Success;
    }
}




