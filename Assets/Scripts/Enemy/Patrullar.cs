using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrullar : MonoBehaviour, IStateEnemy
{
    [SerializeField] Transform waypointRoot;
    [SerializeField] List<Transform> waypointsRefs;
    [SerializeField] NavMeshAgent agent;
    EnemyAnimController animController;
    int currentWayPointIndex;

    private void Awake()
    {
        animController = GetComponent<EnemyAnimController>();

        foreach (Transform waypoint in waypointRoot)
            waypointsRefs.Add(waypoint);

        waypointRoot.SetParent(null);
    }
    public void OnStart()
    {
        agent.isStopped = false;
        animController.PlayAnimation(AnimationsTransition.Patrullar);
        Debug.Log("Patrullar");
    }

    public void OnUpdate()
    {
        agent.SetDestination(waypointsRefs[currentWayPointIndex].position);
        if (Vector3.Distance(transform.position, waypointsRefs[currentWayPointIndex].position) < 3) NextWayPoint();
    }
    public void OnEnd()
    {
        agent.isStopped = true;
    }

    void NextWayPoint()
    {
        currentWayPointIndex++;
        if (currentWayPointIndex >= waypointsRefs.Count) currentWayPointIndex = 0;
    }
}
