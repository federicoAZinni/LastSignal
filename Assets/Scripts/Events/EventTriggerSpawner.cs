using UnityEngine;

public class EventTriggerSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject thingToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entraste en mi area, cosa spoooky cargada");
            thingToLoad.SetActive(true);
        }
    }
}
