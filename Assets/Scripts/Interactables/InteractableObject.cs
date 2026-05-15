using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Canvas canvasUI;
    [SerializeField] Image radialEImg;

    Coroutine animRadialHoldPress;

    public void Interact(bool input)
    {
        if (input && animRadialHoldPress == null)
        {
            animRadialHoldPress = StartCoroutine(AnimRadialHoldPress());
        }
        else if (!input && animRadialHoldPress != null)
        {
            StopCoroutine(animRadialHoldPress);
            animRadialHoldPress = null;

            if (radialEImg != null)
                radialEImg.fillAmount = 0;
        }
    }

    IEnumerator AnimRadialHoldPress()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime;

            if (radialEImg != null)
                radialEImg.fillAmount = time;

            yield return null;
        }

        animRadialHoldPress = null;

        if (radialEImg != null)
            radialEImg.fillAmount = 0;

        OnInteract();
    }

    protected abstract void OnInteract();

    public void ShowUI()
    {
        if (canvasUI != null)
            canvasUI.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        if (canvasUI != null)
            canvasUI.gameObject.SetActive(false);
    }
}