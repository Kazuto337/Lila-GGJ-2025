using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{

    [Header("Movement Properties")]
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference jumpAction;

    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Phisycs Attributes")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float playerMass;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool canJump;
    [SerializeField] private int jumpsAmount = 2;
    [SerializeField] private float secondJumpCooldown = 1;

    private Transform cameraTransmform;

    private void OnEnable()
    {
        movementAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void Start()
    {
        LockCursor();

        controller = gameObject.GetComponent<CharacterController>();
        cameraTransmform = Camera.main.transform;
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        //Jump validation
        bool jumpCondition = jumpsAmount > 0 && canJump;
        bool isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            canJump = true;
            jumpsAmount = 2;
            playerVelocity = Vector3.zero;
        }

        Vector2 movementInputVector = movementAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInputVector.x, 0, movementInputVector.y);
        move = cameraTransmform.forward * move.z + cameraTransmform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Makes the player jump
        if (jumpAction.action.triggered && jumpCondition)
        {
            if (jumpsAmount > 1)
            {
                StartCoroutine(WaitForSecondJump());
            }
            else
            {
                canJump = false;
            }

            jumpsAmount--;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * - 2 * gravityValue);
            controller.Move(playerVelocity * Time.deltaTime);
        }

        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * playerMass * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator WaitForSecondJump()
    {
        canJump = false;

        yield return new WaitForSeconds(secondJumpCooldown);

        canJump = true;
    }

    private void OnDisable()
    {
        movementAction.action.Disable();
        jumpAction.action.Disable();
    }
}
