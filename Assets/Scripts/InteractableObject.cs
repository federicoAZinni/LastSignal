using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    //UI
    [SerializeField] Canvas canvasUI;
    [SerializeField] Image radialEImg;

    Coroutine animRadialHoldPress;

    public void Interact(bool input)
    {
        if (input && animRadialHoldPress == null)
            animRadialHoldPress = StartCoroutine(AnimRadialHoldPress());
        else
        {
            if(animRadialHoldPress!=null)
            {
                StopCoroutine(animRadialHoldPress);
                radialEImg.fillAmount = 0;
            }
        }
            
    }

    IEnumerator AnimRadialHoldPress()
    {
        float time = 0;

        while(time<1)
        {
            time += Time.deltaTime;
            radialEImg.fillAmount = time;
            yield return null;
        }

        OnInteract();

        animRadialHoldPress = null;
    }

    protected abstract void OnInteract();

    public void ShowUI()
    {
        if (canvasUI != null) canvasUI.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        if (canvasUI != null) canvasUI.gameObject.SetActive(false);
    }




}

