using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.OnFinalTrigger();
        }
    }
}
