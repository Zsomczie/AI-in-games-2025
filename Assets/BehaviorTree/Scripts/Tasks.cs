using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public interface ITask //strategy
{
    BT_Node.Status Process();

    void Reset()
    {

    }
}
public class PatrolTask : ITask 
{
    NavMeshAgent agent;
    Transform waypoints;
    List<Transform> waypointList;
    float cooldown;
    int currentIndex;
    bool isPathCalculated;

    public PatrolTask(NavMeshAgent agent, List<Transform> waypointList)
    {
        this.agent = agent;
        this.waypointList = waypointList;
    }

    public BT_Node.Status Process() 
    {
        if (currentIndex == waypointList.Count) 
        {
            return BT_Node.Status.Success;
        }
        var target = waypointList[currentIndex];
        agent.SetDestination(waypointList[currentIndex].position);

        if (agent.pathPending)
        {
            isPathCalculated = true;
        }

        if (isPathCalculated&&agent.remainingDistance<0.1f)
        {
            currentIndex++;
            isPathCalculated = false;
        }

        

        return BT_Node.Status.Running;
    }

    public void Reset() 
    {
        currentIndex = 0;
    }
}

public class Condition : ITask 
{
    Func<bool> conditionFunc;

    public Condition(Func<bool> conditionFunc) 
    {
        this.conditionFunc = conditionFunc;
    }

    public BT_Node.Status Process() 
    {
        if (conditionFunc())
        {
            return BT_Node.Status.Success;
        }
        return BT_Node.Status.Failure;
    }
}
