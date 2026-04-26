
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Splines;
using FMOD.Studio;

public class Anim1Car : MonoBehaviour
{
    [SerializeField] Transform carT;
    [SerializeField] Transform refPosition;
    [SerializeField] AudioSource audioSourceAmbiental;
    [SerializeField] SplineAnimate splineAnimate;
    [SerializeField] Animator animatorCinemachine;
    [SerializeField] CinemachineInputAxisController inputAxisController;
    [SerializeField] Image fadeoutImg;
    [Header("Sound")]
    EventInstance wheelsSoundInstance;
    bool endAnim = false;

    private void Start()
    {
        Player.OnCinematic?.Invoke(true);
        inputAxisController.enabled = false;
        // StartCoroutine(SoundPanMove());
        wheelsSoundInstance = AudioManager.Instance.CreateEventInstance(FmodEvents.Instance.carWheels);
        wheelsSoundInstance.start();
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
        AudioManager.Instance.SetParameterValue("WheelsIntensity", 0,  wheelsSoundInstance);
        AudioManager.Instance.PlayOneshot(FmodEvents.Instance.handBreak, carT.position);
        yield return new WaitForSeconds(1);
        animatorCinemachine.Play("PlayerFirstCamera");
        wheelsSoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.PlayOneshot(FmodEvents.Instance.carDoorOpens, carT.position);
        AudioManager.Instance.SetParameterValue("Intensity", 1,  AudioManager.Instance.ambientSoundInstance);
        yield return new WaitForSeconds(1);
        AudioManager.Instance.PlayOneshot(FmodEvents.Instance.carDoorCloses, carT.position);
        Player.OnCinematic?.Invoke(false);
        yield return new WaitForSeconds(0.1f);
        inputAxisController.enabled = true;
        
    }

    void FadeOut()
    {
        fadeoutImg.CrossFadeAlpha(0, 15f, true);
    }
    
}
