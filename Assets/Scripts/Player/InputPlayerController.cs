using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayerController : MonoBehaviour, IPlayerModule
{
    // Inputs expuestos como propiedades de solo lectura hacia afuera
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool LightToggle { get; private set; }
    public bool Interact { get; private set; }
    public bool Esc { get; private set; }

    public void Init(Player player) { }

    public void OnEsc(InputValue value)
    {
        Esc = !Esc;

        // Notificar a los minijuegos activos que se presiono ESC
        MiniGame.OnEscPressed?.Invoke();
    }

    public void OnInteract(InputValue value) => Interact = !Interact;

    public void OnInteractLight(InputValue value) => LightToggle = !LightToggle;

    public void OnMove(InputValue value) => MovementInput = value.Get<Vector2>();

    public void OnLook(InputValue value) => LookInput = value.Get<Vector2>();
}