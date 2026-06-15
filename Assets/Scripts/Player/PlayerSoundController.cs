using FMOD.Studio;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour, IPlayerModule
{
    private Player player;
    private InputPlayerController input;

    private bool isMoving;
    private EventInstance playerFootSteps;

    public void Init(Player player)
    {
        this.player = player;
        input = player.Input;
        // AudioManager.Instance.SetStepsSound() para cambiar el material de los pasos
    }

    private void Start()
    {
        playerFootSteps = AudioManager.Instance.CreateEventInstance(FmodEvents.Instance.steps);
    }

    private void Update()
    {
        if (player.StunMovement)
        {
            // Frenar los pasos si estaban sonando
            playerFootSteps.stop(STOP_MODE.ALLOWFADEOUT);
            return;
        }

        isMoving = input.MovementInput.magnitude > 0;

        if (isMoving)
        {
            playerFootSteps.getPlaybackState(out PLAYBACK_STATE playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                playerFootSteps.start();
        }
        else
        {
            playerFootSteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}