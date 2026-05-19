using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : Player, IPlayerModule
{
    [Header("Dependencies")]
    [SerializeField] CharacterController cc;

    [Space(5)]
    [Header("Variables")]
    [SerializeField] float speedWalk;
    [SerializeField] float speedRot;
    bool groundedPlayer;
    float gravityValue = -9.81f;
    Vector3 directionMove;
    public Vector3 directionRot;

    public void Init()
    {

    }

    private void Update()
    {
        if (base.stunMovement) return;
        if (base.minigameStunMovement) return;

        if (!base.ladderMovement) MovePlayer();
        else MoveLadder();

        LookRotate();
    }

     void MovePlayer()
    {
        groundedPlayer = cc.isGrounded;
        if (groundedPlayer)
        {
            if (directionMove.y < -2f)
                directionMove.y = -2f;
        }

        directionMove = new Vector3(inputPlayerController.input_playerMove.x, directionMove.y, inputPlayerController.input_playerMove.y) * Time.deltaTime * speedWalk;
        directionMove.y += gravityValue * Time.deltaTime;

        cc.Move(directionMove.x * transform.right + directionMove.z * transform.forward + Vector3.up * directionMove.y);
    }

    void MoveLadder()
    {
        float vertical = inputPlayerController.input_playerMove.y;

        directionMove = Vector3.zero;
        directionMove.y = vertical * speedWalk * Time.deltaTime;

        cc.Move(Vector3.up * directionMove.y);

    }

     void LookRotate()
    {
        directionRot.y += inputPlayerController.input_lookMove.x * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, directionRot.y, transform.eulerAngles.z);
    }
}