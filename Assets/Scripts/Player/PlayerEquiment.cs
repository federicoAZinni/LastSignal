using UnityEngine;

public class PlayerEquiment : MonoBehaviour, IPlayerModule
{
    [SerializeField] private GameObject flashLight;
    [SerializeField] private Transform cameraFirstPlayer;

    private Player player;
    private InputPlayerController input;

    public void Init(Player player)
    {
        this.player = player;
        input = player.Input;
        cameraFirstPlayer = GameObject.FindGameObjectWithTag("FirstCameraPlayer").transform;
    }

    private void Update()
    {
        if (player.StunMovement) return;

        flashLight.SetActive(input.LightToggle);
        Rotation();
    }

    private void Rotation()
    {
        flashLight.transform.rotation = cameraFirstPlayer.rotation;
    }
}