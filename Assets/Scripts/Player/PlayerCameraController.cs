using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour, IPlayerModule
{
    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;

    public void Init(Player player)
    {
        cinemachineInputAxisController = GameObject
            .FindGameObjectWithTag("FirstCameraPlayer")
            .GetComponent<CinemachineInputAxisController>();
    }

    private void OnEnable() => Player.OnMiniGame += ToggleStopMovCamera;

    private void OnDisable() => Player.OnMiniGame -= ToggleStopMovCamera;

    private void ToggleStopMovCamera(bool stopped)
    {
        if (cinemachineInputAxisController != null)
            cinemachineInputAxisController.enabled = !stopped;
    }
}