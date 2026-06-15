using UnityEngine;
using UnityEngine.AI;

public class Alert : MonoBehaviour, IStateEnemy
{
    EnemyAnimController animController;
    [SerializeField] NavMeshAgent agent;
    private void Start()
    {
        animController = GetComponent<EnemyAnimController>();
    }
    public void OnStart()
    {
        animController.PlayAnimation(AnimationsTransition.Alert);
        Debug.Log("Alert");
    }

    public void OnUpdate()
    {
        
    }
    public void OnEnd()
    {
        agent.isStopped = true;
    }
}
