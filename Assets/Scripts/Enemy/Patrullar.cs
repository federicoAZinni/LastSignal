using UnityEngine;
using UnityEngine.AI;

public class Patrullar : MonoBehaviour, IStateEnemy
{
    [SerializeField] Transform[] waypointsRefs;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    int currentWayPointIndex;
    public void OnStart()
    {
        agent.isStopped = false;
        Debug.Log("Patrullar");
    }

    public void OnUpdate()
    {
        agent.SetDestination(waypointsRefs[currentWayPointIndex].position);
        if (Vector3.Distance(transform.position, waypointsRefs[currentWayPointIndex].position) < 1) NextWayPoint();
    }
    public void OnEnd()
    {
        agent.isStopped = true;
    }

    void NextWayPoint()
    {
        currentWayPointIndex++;
        if (currentWayPointIndex >= waypointsRefs.Length) currentWayPointIndex = 0;
    }
}
