using System.Collections;
using UnityEngine;

public class FlashLightItem : MonoBehaviour
{
    Light flashlight;
    private void Awake()
    {
        flashlight = transform.GetChild(0).GetComponent<Light>();
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(20f,50f));

        flashlight.enabled = false;
        yield return new WaitForSeconds(0.1f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.1f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.05f);
        flashlight.enabled = true;


        StartCoroutine(Start());

    }

  
}
