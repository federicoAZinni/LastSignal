using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraController : Player, IPlayerModule
{
    [SerializeField] CinemachineInputAxisController cinemachineInputAxisController;

    private void OnEnable()
    {
        OnCinematic += ToggleStopMovCamera;
    }

    private void OnDisable()
    {
        OnCinematic -= ToggleStopMovCamera;
    }

    public void Init()
    {
        cinemachineInputAxisController = FindFirstObjectByType<CinemachineInputAxisController>();
    }

    void ToggleStopMovCamera(bool n)
    {
        cinemachineInputAxisController.enabled = !n;
    }

}
