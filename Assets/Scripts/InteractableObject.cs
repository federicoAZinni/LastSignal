using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    //UI
    Canvas canvasUI;
    Image radialEImg;

    Coroutine animRadialHoldPress;

    private void Awake()
    {
        canvasUI = transform.GetChild(0).GetComponent<Canvas>();
        radialEImg = canvasUI.transform.GetChild(0).GetComponent<Image>();
    }

    public void Interact(bool input)
    {
        if (input && animRadialHoldPress == null)
            animRadialHoldPress = StartCoroutine(AnimRadialHoldPress());
        else if (animRadialHoldPress!=null && !input)
        { 
             StopCoroutine(animRadialHoldPress);
             radialEImg.fillAmount = 0;
           
           if(!input) animRadialHoldPress = null;
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

