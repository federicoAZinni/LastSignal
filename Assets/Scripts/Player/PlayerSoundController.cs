using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PlayerSoundController : Player, IPlayerModule
{
    [SerializeField] AudioSource playeraudioSource;
    [SerializeField] AudioClip[] footstepClips;
    private bool isMoving;

    private float stepTimer;
    [SerializeField] private float stepInterval = 0.5f;
    public void Init()
    {
        playeraudioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        playeraudioSource.pitch = Random.Range(0.9f, 1.1f); // Variación sutil
        playeraudioSource.PlayOneShot(clip);
    }


    private void Update()
    {

        if (stunMovement) return;
        isMoving = inputPlayerController.input_playerMove.magnitude > 0;
        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
            stepTimer = 0f;
    }


}
