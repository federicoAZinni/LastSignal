using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayerController : Player, IPlayerModule
{
    public Vector2 input_playerMove;
    public Vector2 input_lookMove;
    public bool input_lightToggle;
    public bool input_interact;

    public void Init()
    {

    }

    public void OnInteract(InputValue value)
    {
        input_interact = !input_interact;
    }

    public void OnInteractLight(InputValue value)
    {
        input_lightToggle = !input_lightToggle;
    }
    public void OnMove(InputValue value)
    {
        input_playerMove = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        input_lookMove = value.Get<Vector2>();
    }
}
