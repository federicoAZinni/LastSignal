using FMOD.Studio;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PlayerSoundController : Player, IPlayerModule
{
    
    private bool isMoving;

    private EventInstance playerFootSteps;

    public void Init()
    {
       
    }

    public void Start()
    {
         playerFootSteps = AudioManager.Instance.CreateEventInstance(FmodEvents.Instance.steps);
    }

    public void PlayFootstep()
    {
        
    }


    private void Update()
    {
        isMoving = inputPlayerController.input_playerMove.magnitude > 0;

        if (isMoving)
        {
            PLAYBACK_STATE playback_state;
            playerFootSteps.getPlaybackState(out playback_state);
            if (playback_state.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootSteps.start();
            }
        }
        else
        {
            playerFootSteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
           
    }


}
