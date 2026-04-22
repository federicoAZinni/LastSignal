using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Referencias dependencias")]
    protected InputPlayerController inputPlayerController;
    protected PlayerMovementController playerMovementController;
    protected PlayerSoundController soundController;
    protected PlayerInteractor playerInteractor;

    IPlayerModule[] playerModules;

    public bool stunMovement = true;

    //Eventos
    public static Action<bool> OnCinematic;


    //Init
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
        }
    }
    void InitModules() { foreach (IPlayerModule playerModule in playerModules) playerModule.Init(); }

    //Eventos
    private void OnEnable()
    {
        OnCinematic += OnCinematicPlayer;
    }
    private void OnDisable()
    {
        OnCinematic -= OnCinematicPlayer;
    }

    void OnCinematicPlayer(bool n)
    {
        stunMovement = n;
    }

}


public interface IPlayerModule
{
    public void Init();
}
