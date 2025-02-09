using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference fireAction;
    [SerializeField] InputActionReference nextBubble;
    [SerializeField] InputActionReference previousBubble;

    [SerializeField] Animator animator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Transform cameraTransmform;

    [Header("Phisycs Attributes")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float playerMass;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool canJump;
    [SerializeField] private int jumpsAmount = 2;
    [SerializeField] private float secondJumpCooldown = 1;

    [Header("Brush Attributes")]
    [SerializeField] private bool isLocking;
    [SerializeField] private Transform bubbleSpawnerTransform;
    [SerializeField] private Transform lockedTransform;
    [SerializeField] BubbleType selectedBubble;
    [SerializeField] private int selectedBubbleIndex;
    [SerializeField] List<BubbleType> unlockedBubbles;
    [SerializeField] LayerMask groundRaycastLayer;

    private bool isBouncing;

    [Header("Bubble Prefabs")]
    [SerializeField] GameObject regularBubblePrefab;
    [SerializeField] GameObject explosiveBubblePrefab;
    [SerializeField] GameObject bubbleGumPrefab;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CheckpointSystem checkpointSystem;
    private bool isHealing;
    private bool isCollectingCoin;
    private bool receivingDamage;

    [Header("Brush Selector")]
    [SerializeField] private BrushSelector brushSelector;

    public List<BubbleType> UnlockedBubbles { get => unlockedBubbles; }

    private void OnEnable()
    {
        movementAction.action.Enable();
        jumpAction.action.Enable();

        checkpointSystem.OnBoundaryFound.AddListener(playerStats.TakeDamage);
    }

    private void Start()
    {
        LockCursor();

        brushSelector.DesactivateCurrentBubbleImage();
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransmform = Camera.main.transform;
    }

    void Update()
    {
        Move();
        CheckInventoryInput();
        Fire();
    }

    [ContextMenu("Fire Bubble")]
    private void Fire()
    {
        if (!fireAction.action.triggered)
        {
            return;
        }

        if (unlockedBubbles.Count == 0)
        {
            brushSelector.DesactivateCurrentBubbleImage();
            return;
        }

        //animator.SetBool("isShooting", true);

        if (isLocking)
        {
            FireLocking(lockedTransform);
            return;
        }

        brushSelector.ActivateCurrentBubbleImage();
        Bubble newBubble = null;

        switch (selectedBubble)
        {
            case BubbleType.Regular:
                newBubble = Instantiate(regularBubblePrefab, bubbleSpawnerTransform.position, regularBubblePrefab.transform.rotation).GetComponent<Bubble>();
                break;
            case BubbleType.Gum:
                newBubble = Instantiate(bubbleGumPrefab, bubbleSpawnerTransform.position, regularBubblePrefab.transform.rotation).GetComponent<Bubble>();
                newBubble.FireBubble(GetFloorPosition());
                return;
            case BubbleType.Explosive:
                newBubble = Instantiate(explosiveBubblePrefab, bubbleSpawnerTransform.position, regularBubblePrefab.transform.rotation).GetComponent<Bubble>();
                break;
        }

        newBubble.FireBubble(transform.forward);
    }

    private void FireLocking(Transform lockedObject)
    {

    }

    private Vector3 GetFloorPosition()
    {
        RaycastHit hit;

        Vector3 origin = transform.position - new Vector3(0, controller.height / 2, 0);

        float raycastMaxDistance = 100f;

        if (Physics.Raycast(origin, Vector3.down, out hit, raycastMaxDistance, groundRaycastLayer))
        {
            return hit.point;
        }

        return transform.up * -1;
    }

    private void Move()
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
            
            //animator.SetBool("isMoving", true);
            gameObject.transform.forward = move;
        }
        else
        {
            //animator.SetBool("isMoving", false);
        }

        // Makes the player jump
        if (jumpAction.action.triggered && jumpCondition)
        {
            //animator.SetBool("isJumping", true);
            if (jumpsAmount > 1)
            {
                StartCoroutine(WaitForSecondJump());
            }
            else
            {
                canJump = false;
            }

            jumpsAmount--;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravityValue);
            controller.Move(playerVelocity * Time.deltaTime);
        }

        if (!isGrounded)
        {
            playerVelocity.y += gravityValue * playerMass * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            //animator.SetBool("isJumping", false);
        }
    }

    public IEnumerator Bounce()
    {
        if (isBouncing)
        {
            yield break;
        }


        isBouncing = true;

        yield return new WaitForSeconds(0.05f);

        //animator.SetBool("isJumping", true);

        Debug.Log("Boing");

        playerVelocity.y += Mathf.Sqrt(jumpHeight + 4 * -2 * gravityValue);
        controller.Move(playerVelocity * Time.deltaTime);

        isBouncing = false;
        //animator.SetBool("isJumping", false);
    }

    private void CheckInventoryInput()
    {
        bool previous = previousBubble.action.triggered;
        bool next = nextBubble.action.triggered;

        if (previous && next)
        {
            return;
        }

        if (!previous && !next)
        {
            return;
        }

        if (previous)
        {
            SelectBubble(-1);
            return;
        }

        SelectBubble(1);
    }

    private void SelectBubble(int valueIndex)
    {
        if (unlockedBubbles.Count <= 1)
        {
            return;
        }

        selectedBubbleIndex = Math.Abs((selectedBubbleIndex % unlockedBubbles.Count) + valueIndex);

        if (selectedBubbleIndex == 0) selectedBubbleIndex = Math.Abs(valueIndex + selectedBubbleIndex);

        selectedBubble = unlockedBubbles[selectedBubbleIndex - 1];

        brushSelector.ChangeCurrentBrush(selectedBubble);
    }

    public void UnlockBubble(BubbleType newBubble)
    {
        unlockedBubbles.Add(newBubble);
        selectedBubbleIndex = unlockedBubbles.Count;

        selectedBubble = unlockedBubbles[selectedBubbleIndex - 1];
        brushSelector.ChangeCurrentBrush(selectedBubble);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Spring"))
        {
            Destroy(hit.gameObject);
            StartCoroutine(Bounce());
        }

        if (hit.gameObject.CompareTag("Enemy"))
        {
            float offset = controller.height * 0.75f;

            float enemyHeight = hit.transform.position.y;
            float myHeight = transform.position.y;

            if (enemyHeight + offset < myHeight)
            {
                Destroy(hit.gameObject);
                StartCoroutine(CollectCoin());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble"))
        {

            BubbleType type = other.GetComponent<UnlockBubble>().bubbleType;
            UnlockBubble(type);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Heal"))
        {
            Destroy(other.gameObject);

            StartCoroutine(HealPlayer());
        }

        if (other.CompareTag("WinCollider"))
        {
            GameManager.instance.EndGame();
        }
    }
    private IEnumerator HealPlayer()
    {
        if (isHealing)
        {
            yield break;
        }

        isHealing = true;

        yield return null;
        playerStats.Heal(1);

        yield return new WaitForSeconds(0.5f);

        isHealing = false;
    }
    private IEnumerator CollectCoin()
    {
        if (isCollectingCoin)
        {
            yield break;
        }

        isCollectingCoin = true;

        yield return null;
        playerStats.CollectCoin(5);

        yield return new WaitForSeconds(0.5f);

        isCollectingCoin = false;
    }

    private void OnDisable()
    {
        movementAction.action.Disable();
        jumpAction.action.Disable();
    }
}
public enum BubbleType
{
    Regular,
    Gum,
    Explosive
}
