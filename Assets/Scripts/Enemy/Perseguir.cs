using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Perseguir : MonoBehaviour, IStateEnemy
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    EnemyAnimController animController;
    bool startRun;
    float speedPreset;
    
    private void Start()
    {
        animController = GetComponent<EnemyAnimController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        speedPreset = agent.speed;
    }
    public void OnStart()
    {
        animController.PlayAnimation(AnimationsTransition.Perseguir);
        StartCoroutine(WaitAnimRoar());
    }
    IEnumerator WaitAnimRoar()
    {
        yield return new WaitForSeconds(4.5f);
        startRun = true;
        agent.speed = speedPreset * 2;
        agent.isStopped = false;
    }

    public void OnUpdate()
    {
        if (!startRun) return;
        agent.SetDestination(target.position);
    }
    public void OnEnd()
    {
        agent.isStopped = true;
        startRun = false;
        agent.speed = speedPreset;
    }
}
