using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform waypoints1,waypoints2;
    [SerializeField] List<Transform> waypointList1;
    [SerializeField] List<Transform> waypointList2;
    BehaviorTree BehaviorTree;
    Transform safePoint1;
    Transform safePoint2;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < waypoints1.childCount; i++)
        {
            waypointList1.Add(waypoints1.GetChild(i));
        }
        for (int i = 0; i < waypoints2.childCount; i++)
        {
            waypointList2.Add(waypoints2.GetChild(i));
        }

        BehaviorTree = new BehaviorTree("BehaviorTree");

        Selector PatrolSelector = new Selector("SafePointSelector");

        Sequence PatrolSequence1 = new Sequence("SafePointSequence1");
        PatrolSequence1.AddChild(new Leaf("isPatrol1?", new Condition(() => waypointList1.Count > waypointList2.Count)));
        PatrolSequence1.AddChild(new Leaf("PatrolAction1", new PatrolTask(agent, waypointList1)));

            Sequence PatrolSequence2 = new Sequence("SafePointSequence2");
        PatrolSequence2.AddChild(new Leaf("isPatrol2?", new Condition(() => waypointList2.Count > waypointList1.Count)));
        PatrolSequence2.AddChild(new Leaf("PatrolAction2", new PatrolTask(agent, waypointList2)));

        PatrolSelector.AddChild(PatrolSequence1);
        PatrolSelector.AddChild(PatrolSequence2);
        //Sequence PatrolSequence = new Sequence("PatrolSequence");
        //PatrolSequence.AddChild(new Leaf("PatrolCondition", new Condition(() => waypointList.Count > 1)));
        //PatrolSequence.AddChild(new Leaf("PatrolAction", new PatrolTask(agent, waypointList)));

        BehaviorTree.AddChild(PatrolSelector);

        //BehaviorTree.AddChild(PatrolSequence);
    }

    private void Update()
    {
        if (BehaviorTree.Process()==BT_Node.Status.Success)
        {
            BehaviorTree.Reset();
        }
    }
}
