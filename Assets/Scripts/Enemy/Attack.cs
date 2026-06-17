using UnityEngine;

public class Attack : MonoBehaviour, IStateEnemy
{
    public void OnEnd()
    {
        
    }

    public void OnStart()
    {
        GameManager.instance.PlayerDead();
    }

    public void OnUpdate()
    {
       
    }

   
}
