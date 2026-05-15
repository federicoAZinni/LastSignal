using UnityEngine;

public class PanelElectrico : InteractableObject
{
    [SerializeField] MiniGame minigame;

    protected override void OnInteract()
    {
        minigame.Open();
    }
}
