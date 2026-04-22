using UnityEngine;

public class PlayerEquiment : Player, IPlayerModule
{

    [SerializeField] GameObject flashLight;
    [SerializeField] Transform cameraFirstPlayer;
    public void Init()
    {
        cameraFirstPlayer = GameObject.FindGameObjectWithTag("FirstCameraPlayer").transform;
    }

    private void Update()
    {
        flashLight.SetActive(inputPlayerController.input_lightToggle);
        Rotation();
    }

    void Rotation()
    {
        flashLight.transform.rotation = cameraFirstPlayer.rotation;
    }

}
