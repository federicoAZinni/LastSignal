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
    //float jumpHeight = 1.5f;
    float gravityValue = -9.81f;
    Vector3 directionMove;
    public Vector3 directionRot;

    public void Init()
    {
       
    }

    private void Update()
    {
        if (stunMovement) return;

        MovePlayer();
        LookRotate();
    }
    public void MovePlayer()
    {
        groundedPlayer = cc.isGrounded;
        if (groundedPlayer)
        {
            if (directionMove.y < -2f)
                directionMove.y = -2f;
        }


        // Jump 
        //if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        //{
        //    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        //}


        directionMove = new Vector3(inputPlayerController.input_playerMove.x, directionMove.y, inputPlayerController.input_playerMove.y) * Time.deltaTime *speedWalk;

        directionMove.y += gravityValue * Time.deltaTime;

        cc.Move(directionMove.x * transform.right + directionMove.z * transform.forward + Vector3.up * directionMove.y);
    }

    public void LookRotate()
    {
       directionRot.y += inputPlayerController.input_lookMove.x * Time.deltaTime;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, directionRot.y, transform.eulerAngles.z); 

    }

}
