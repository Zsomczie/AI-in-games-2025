using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : BT_Node
{
    ITask task;

    public Leaf(string name, ITask task) : base(name)
    {
        this.task = task;
    }

    public override Status Process()
    {
        return task.Process();
    }

    public override void Reset()
    {
        task.Reset();
    }
}
