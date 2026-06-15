using UnityEngine;

public class PlayerMovementController : MonoBehaviour, IPlayerModule
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController cc;

    [Space(5)]
    [Header("Variables")]
    [SerializeField] private float speedWalk;
    [SerializeField] private float speedRot;

    private Player player;
    private InputPlayerController input;

    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    private Vector3 directionMove;
    private Vector3 directionRot;

    public Vector3 DirectionRot => directionRot;

    public void Init(Player player)
    {
        this.player = player;
        input = player.Input;
    }

    private void Update()
    {
        if (player.StunMovement) return;
        if (player.MinigameStunMovement) return;

        if (!player.LadderMovement) MovePlayer();
        else MoveLadder();

        LookRotate();
    }

    private void MovePlayer()
    {
        groundedPlayer = cc.isGrounded;
        if (groundedPlayer)
        {
            if (directionMove.y < -2f)
                directionMove.y = -2f;
        }

        directionMove = new Vector3(input.MovementInput.x, directionMove.y, input.MovementInput.y)
                        * Time.deltaTime * speedWalk;
        directionMove.y += gravityValue * Time.deltaTime;

        cc.Move(directionMove.x * transform.right
              + directionMove.z * transform.forward
              + Vector3.up * directionMove.y);
    }

    private void MoveLadder()
    {
        float vertical = input.MovementInput.y;

        directionMove = Vector3.zero;
        directionMove.y = vertical * speedWalk * Time.deltaTime;

        cc.Move(Vector3.up * directionMove.y);
    }

    private void LookRotate()
    {
        directionRot.y += input.LookInput.x * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, directionRot.y, transform.eulerAngles.z);
    }
}