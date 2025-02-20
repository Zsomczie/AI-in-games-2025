using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform waypoints;
    [SerializeField] List<Transform> waypointList;
    BehaviorTree BehaviorTree;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < waypoints.childCount; i++)
        {
            waypointList.Add(waypoints.GetChild(i));
        }

        BehaviorTree = new BehaviorTree("BehaviorTree");

        Sequence PatrolSequence = new Sequence("PatrolSequence");
        PatrolSequence.AddChild(new Leaf("PatrolCondition", new Condition(() => waypointList.Count > 1)));
        PatrolSequence.AddChild(new Leaf("PatrolAction", new PatrolTask(agent, waypointList)));

        BehaviorTree.AddChild(PatrolSequence);
    }

    private void Update()
    {
        if (BehaviorTree.Process()==BT_Node.Status.Success)
        {
            BehaviorTree.Reset();
        }
    }
}
