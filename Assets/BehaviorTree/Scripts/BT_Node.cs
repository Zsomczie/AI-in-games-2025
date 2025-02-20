using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Node 
{
    public enum Status 
    {
        Success,
        Failure,
        Running
    }

    public string name;
    
    public List<BT_Node> children = new List<BT_Node>();
    public int currentChild;

    public BT_Node (string name)
    {
        this.name = name;
    }

    public void AddChild(BT_Node child) 
    {
        children.Add(child);
    }

    public virtual void Reset() 
    {
        currentChild = 0;
        foreach (BT_Node child in children) 
        {
            child.Reset();
        }
    }

    public virtual Status Process() 
    {
        return children[currentChild].Process();
    }
}

public class BehaviorTree : BT_Node 
{
    public BehaviorTree(string name) : base(name) { }

    public override Status Process()
    {
        while (currentChild<children.Count) 
        {
            var status = children[currentChild].Process();
            if (status!=Status.Success)
            {
                return status;
            }

            currentChild++;
        }
        return Status.Success;
    }
}

public class Sequence : BT_Node 
{
    public Sequence(string name) : base(name) { }

    public override Status Process()
    {
        if (currentChild<children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.Failure:
                    Reset();
                    return Status.Failure;
                case Status.Running:
                    return Status.Running;
                default:
                    currentChild++;
                    if (currentChild==children.Count)
                    {
                        return Status.Success;
                    }
                    else
                    {
                        return Status.Running;
                    }
            }
        }
        Reset();
        return Status.Success;
    }
}