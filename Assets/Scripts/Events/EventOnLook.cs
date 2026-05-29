using UnityEngine;
using System.Collections;

public class EventOnLook : MonoBehaviour
{
    private Coroutine coroutine;
    // Tal vez seria mejor hacer que ereden todos de un evento y cada uno se especialize en su forma de activarse pero esto funciona por ahora
    private void OnBecameVisible()
    {
        coroutine = StartCoroutine(DisapearTimer());
    }

    private IEnumerator DisapearTimer()
    {
        Debug.Log("Me viste? Pues disfrutalo, no durara mucho.");
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }
}
