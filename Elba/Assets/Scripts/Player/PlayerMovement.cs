using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;

    [Header("References")]
    [SerializeField] private PlayerStatsSystem stats;
    [SerializeField] private PlayerStaminaSystem staminaSystem;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;

    private Vector2 movementInput;
    private Vector2 lastDirection = Vector2.down;

    private bool movementLocked;

    public Vector2 MovementInput => movementInput;
    public Vector2 LastDirection => lastDirection;
    public bool IsMoving => movementInput != Vector2.zero;
    public bool IsMovementLocked => movementLocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();

        if (staminaSystem == null)
            staminaSystem = GetComponent<PlayerStaminaSystem>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (movementLocked)
        {
            movementInput = Vector2.zero;
            staminaSystem.SetSprinting(false);
            UpdateAnimator();
            return;
        }

        ReadInput();
        HandleSprint();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (movementLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Move();
    }

    private void ReadInput()
    {
        movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized;

        if (movementInput != Vector2.zero)
            lastDirection = movementInput;
    }

    private void HandleSprint()
    {
        bool wantsSprint = Input.GetKey(KeyCode.LeftShift);

        bool canSprint =
            wantsSprint &&
            movementInput != Vector2.zero &&
            staminaSystem.CanSprint();

        staminaSystem.SetSprinting(canSprint);
    }

    private void Move()
    {
        rb.linearVelocity =
            movementInput *
            GetCurrentSpeed() *
            GetMovementMultiplier();
    }

    private float GetCurrentSpeed()
    {
        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) &&
            movementInput != Vector2.zero &&
            staminaSystem.CanSprint();

        return sprinting
            ? sprintSpeed
            : walkSpeed;
    }

    private float GetMovementMultiplier()
    {
        float multiplier = 1f;

        if (stats.IsLow(StatType.Hunger))
            multiplier *= 0.8f;

        if (stats.IsLow(StatType.Thirst))
            multiplier *= 0.8f;

        return multiplier;
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        animator.SetBool("isMoving", movementInput != Vector2.zero);

        Vector2 direction =
            movementInput != Vector2.zero
            ? movementInput
            : lastDirection;

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    public void SetMovementLocked(bool value)
    {
        movementLocked = value;

        if (value)
        {
            movementInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            staminaSystem.SetSprinting(false);

            UpdateAnimator();
        }
    }

    public void ForceStop()
    {
        movementInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        staminaSystem.SetSprinting(false);

        UpdateAnimator();
    }

    public float GetCurrentMoveSpeed()
    {
        return GetCurrentSpeed() * GetMovementMultiplier();
    }
}