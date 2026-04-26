using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { private set; get;}
    public EventInstance ambientSoundInstance;

    void Awake()
    {
        if (Instance!=null&&Instance!=this)
        {
            Debug.LogWarning("More than one AudioManager");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        ambientSoundInstance = CreateEventInstance(FmodEvents.Instance.ambience);
        ambientSoundInstance.start();
        SetParameterValue("Intensity", 0, ambientSoundInstance);
    }

    public void SetParameterValue(string parameterName, float parameterValueTarget, EventInstance eventInstance)
    {
        eventInstance.setParameterByName(parameterName, parameterValueTarget);
    }
    public void PlayOneshot(EventReference sound, Vector3 pos)   
    {
        RuntimeManager.PlayOneShot(sound, pos);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        return eventInstance;
    }

}
