using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool StunMovement { get; private set; } = false;   
    public bool LadderMovement { get; private set; } = false; 
    public bool MinigameStunMovement { get; private set; } = false; 

  
    public InputPlayerController Input { get; private set; }

    private IPlayerModule[] playerModules;

    // --- Eventos ---
    public static Action<bool> OnCinematic;
    public static Action<bool> OnLadder;
    public static Action<bool> OnMiniGame;

    private void Awake()
    {
        playerModules = GetComponentsInChildren<IPlayerModule>();
        ResolveReferences();
        InitModules();
    }

    private void ResolveReferences()
    {
        foreach (IPlayerModule module in playerModules)
        {
            if (module is InputPlayerController input)
                Input = input;
        }
    }

    private void InitModules()
    {
        foreach (IPlayerModule module in playerModules)
            module.Init(this);
    }


    public T GetModule<T>() where T : class, IPlayerModule
    {
        foreach (IPlayerModule module in playerModules)
        {
            if (module is T match)
                return match;
        }
        return null;
    }

    private void OnEnable()
    {
        OnCinematic += HandleCinematic;
        OnLadder += HandleLadder;
        OnMiniGame += HandleMiniGame;
    }

    private void OnDisable()
    {
        OnCinematic -= HandleCinematic;
        OnLadder -= HandleLadder;
        OnMiniGame -= HandleMiniGame;
    }

    private void HandleCinematic(bool value) => StunMovement = value;
    private void HandleLadder(bool value) => LadderMovement = value;
    private void HandleMiniGame(bool value) => MinigameStunMovement = value;
}

public interface IPlayerModule
{
    void Init(Player player);
}