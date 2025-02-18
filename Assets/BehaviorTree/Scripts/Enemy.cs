using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AIDetection))]
public class Enemy : MonoBehaviour
{
    [SerializeField] Transform waypointParent;
    [SerializeField]List<Transform> waypoints;
    AIDetection AIDetection;
    NavMeshAgent agent;
    BehaviorTree BehaviorTree;
    [SerializeField]Transform playerTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < waypointParent.childCount; i++) 
        {
            waypoints.Add(waypointParent.GetChild(i).transform);
        }
        AIDetection = GetComponent<AIDetection>();

        BehaviorTree = new BehaviorTree("BT_Enemy");

       

        Sequence PatrolSequence = new Sequence("PatrolSequence");
        PatrolSequence.AddChild(new Leaf("isPatrolling", new Condition(() => waypoints.Count>0)));
        PatrolSequence.AddChild(new Leaf("Patrol", new PatrolTask(transform,agent,waypoints)));

        Sequence ChasePlayerSequence = new Sequence("ChasePlayerSequence");
        Leaf isChasing = new Leaf("isChasing", new Condition(() => AIDetection.playerVisible));
        Leaf ChasePlayer = new Leaf("ChasePlayer", new ChaseTask(AIDetection, playerTarget, agent));
        ChasePlayerSequence.AddChild(isChasing);
        ChasePlayerSequence.AddChild(ChasePlayer);

        Selector ChaseOrPatrol = new Selector("ChaseOrPatrol");
        ChaseOrPatrol.AddChild(PatrolSequence);
        ChaseOrPatrol.AddChild(ChasePlayerSequence);



        BehaviorTree.AddChild(PatrolSequence);
        BehaviorTree.AddChild(ChasePlayerSequence);


        //BehaviorTree.AddChild(new Leaf("Patrol", new PatrolTask(transform, agent, waypoints)));
        //BehaviorTree.AddChild(new Leaf("isChasing", new Condition(() => AIDetection.playerVisible)));
        //BehaviorTree.AddChild(new Leaf("ChasePlayer", new ChaseTask(AIDetection, playerTarget, agent)));
    }

    private void Update()
    {
        BehaviorTree.Process();
        //if (BehaviorTree.Process()==BT_Node.Status.Success)
        //{
        //    BehaviorTree.Reset();
        //}

       
    }
}
