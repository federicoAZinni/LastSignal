using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Referencias dependencias")]
    protected InputPlayerController inputPlayerController;
    protected PlayerMovementController playerMovementController;
    protected PlayerSoundController soundController;
    protected PlayerInteractor playerInteractor;
    protected PlayerCameraController cameraController;

    IPlayerModule[] playerModules;

    public bool stunMovement = true;
    public bool ladderMovement = false;

    // Evento estático: cualquier sistema puede invocar Player.OnCinematic(true/false)
    // para bloquear/desbloquear el movimiento del player.
    public static Action<bool> OnCinematic;
    public static Action<bool> OnLadder;

    void Awake()
    {
        playerModules = transform.GetComponentsInChildren<IPlayerModule>();
        SetReferencies();
        InitModules();
    }

    void SetReferencies()
    {
        foreach (IPlayerModule playerModule in playerModules)
        {
            if (playerModule is InputPlayerController ipc)
                inputPlayerController = ipc;
            else if (playerModule is PlayerMovementController pmc)
                playerMovementController = pmc;
            else if (playerModule is PlayerSoundController psc)
                soundController = psc;
            else if (playerModule is PlayerInteractor pi)
                playerInteractor = pi;
            else if (playerModule is PlayerCameraController pcc)
                cameraController = pcc;
        }
    }

    void InitModules()
    {
        foreach (IPlayerModule playerModule in playerModules)
            playerModule.Init();
    }

    private void OnEnable()
    {
        OnCinematic += OnCinematicPlayer;
        OnLadder += OnStairPlayer;
    }

    private void OnDisable()
    {
        OnCinematic -= OnCinematicPlayer;
        OnLadder -= OnStairPlayer;
    }

    void OnCinematicPlayer(bool n) => stunMovement = n;
    void OnStairPlayer(bool n) => ladderMovement = n;


}

public interface IPlayerModule
{
    public void Init();
}
