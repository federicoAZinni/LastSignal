
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;

public class Anim1Car : MonoBehaviour
{
    [SerializeField] Transform carT;
    [SerializeField] Transform refPosition;
    [SerializeField] AudioSource audioSourceAmbiental;
    [SerializeField] SplineAnimate splineAnimate;
    [SerializeField] Animator animatorCinemachine;
    [SerializeField] CinemachineInputAxisController inputAxisController;
    [SerializeField] Image fadeoutImg;
    bool endAnim = false;

    private void Start()
    {
        Player.OnCinematic?.Invoke(true);
        inputAxisController.enabled = false;
        StartCoroutine(SoundPanMove());
        FadeOut();
    }

    private void Update()
    {
        if (endAnim) return;

        Vector3 tempsinY = new Vector3(refPosition.position.x,
                                        carT.position.y,
                                        refPosition.position.z);

        carT.position = Vector3.Lerp(carT.position, tempsinY, Time.deltaTime);

        Quaternion tempSoloY = Quaternion.Euler(carT.eulerAngles.x,
                                refPosition.eulerAngles.y,
                                carT.eulerAngles.z);

        carT.rotation = Quaternion.Lerp(carT.rotation, tempSoloY, Time.deltaTime ); ;

        if (!splineAnimate.IsPlaying)
        {
            if(Vector3.Distance(carT.position, tempsinY)<0.5f)
            {
                StartCoroutine(EndOfAnim());
                endAnim = true;
            }
        }

    }

    IEnumerator SoundPanMove()
    {
        float current = audioSourceAmbiental.panStereo;
        float random = Random.Range(-0.8f, 0.8f);
        float time = 0;

        while (time <= 1)
        {
            time += Time.deltaTime * 0.5f;
            audioSourceAmbiental.panStereo = Mathf.Lerp(current, random, time);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(SoundPanMove());
    }

    IEnumerator EndOfAnim()
    {
        yield return new WaitForSeconds(1);
        animatorCinemachine.Play("PlayerFirstCamera");
        yield return new WaitForSeconds(1);
        Player.OnCinematic?.Invoke(false);
        yield return new WaitForSeconds(0.1f);
        inputAxisController.enabled = true;
    }

    void FadeOut()
    {
        fadeoutImg.CrossFadeAlpha(0, 15f, true);
    }
    
}
