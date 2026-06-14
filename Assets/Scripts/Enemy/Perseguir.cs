using UnityEngine;
using UnityEngine.AI;

public class Perseguir : MonoBehaviour, IStateEnemy
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    public void OnStart()
    {
        agent.isStopped = false;
        Debug.Log("Perseguir");
    }

    public void OnUpdate()
    {
        agent.SetDestination(target.position);
    }
    public void OnEnd()
    {
        agent.isStopped = true;
        
    }
}
