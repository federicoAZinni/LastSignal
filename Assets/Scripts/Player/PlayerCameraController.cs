using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraController : Player, IPlayerModule
{
    [SerializeField] CinemachineInputAxisController cinemachineInputAxisController;

    private void OnEnable()
    {
        OnMiniGame += ToggleStopMovCamera;
    }

    private void OnDisable()
    {
        OnMiniGame -= ToggleStopMovCamera;
    }

    public void Init()
    {
        cinemachineInputAxisController = GameObject.FindGameObjectWithTag("FirstCameraPlayer").GetComponent<CinemachineInputAxisController>();
    }

    void ToggleStopMovCamera(bool n)
    {
        Debug.Log(n);
        cinemachineInputAxisController.enabled = !n;
    }

}
