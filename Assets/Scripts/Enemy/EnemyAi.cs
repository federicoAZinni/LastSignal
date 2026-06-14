
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] Transform target;
    [Range(-1,1)]
    [SerializeField] float amplitudVision;
    [SerializeField] float distanciaVisionPerseguir;
    [SerializeField] float distanciaAlert;

    IStateEnemy currentState;

    IStateEnemy[] states;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        InitInstanceOfStates();
        ChangeState(GetState<Patrullar>());
    }

    void InitInstanceOfStates()
    {
        states = gameObject.GetComponents<IStateEnemy>();
    }

    T GetState<T>() where T : class, IStateEnemy
    {
        foreach (var state in states)
        {
            if (state is T match)
                return match;
        }
        return null;
    }

    void ChangeState(IStateEnemy newState)
    {
        if (newState == currentState) return; 

        currentState?.OnEnd();
        currentState = newState;
        currentState.OnStart();
    }


    void Update()
    {
        ChangeState(DecideState());   
        currentState.OnUpdate();      
    }

    IStateEnemy DecideState()
    {
        if (!CanSeePlayer(out float distance)) return GetState<Patrullar>();

        return distance <= distanciaVisionPerseguir ? GetState<Perseguir>() : GetState<Alert>();
        
    }

    bool CanSeePlayer(out float distance)
    {
        distance = Mathf.Infinity;

        Vector3 dir = (target.position - transform.position).normalized;

        if (Vector3.Dot(transform.forward, dir) < amplitudVision) return false;

        if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, distanciaAlert))
            return false;

        if (!hit.collider.CompareTag("Player")) return false;

        distance = hit.distance;
        return true;
    }

}


public interface IStateEnemy
{
    public void OnStart();
    public void OnUpdate();
    public void OnEnd();
}
