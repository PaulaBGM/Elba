using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;

    [Header("References")]
    [SerializeField] private PlayerStatsSystem stats;
    [SerializeField] private PlayerStaminaSystem staminaSystem;
    [SerializeField] private PlayerAttackSystem attackSystem;
    [SerializeField] private Animator animator;

    [Header("Knockback")]
    [SerializeField] private float knockbackRecoveryTime = 0.2f;

    private Coroutine knockbackRoutine;
    private Rigidbody2D rb;

    private Vector2 movementInput;
    private Vector2 lastDirection = Vector2.down;

    private bool movementLocked;
    private bool isKnockedBack;

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

        if (attackSystem == null)
            attackSystem = GetComponent<PlayerAttackSystem>();

        if (input == null)
            input = FindFirstObjectByType<InputReader>();
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

        if (attackSystem != null && attackSystem.IsAttacking)
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

        if (isKnockedBack)
            return;

        if (attackSystem != null && attackSystem.IsAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Move();

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, bottomLeft.position.x, topRight.position.x);
        position.y = Mathf.Clamp(position.y, bottomLeft.position.y, topRight.position.y);

        transform.position = position;
    }

    private void ReadInput()
    {
        movementInput = input.MoveInput.normalized;

        if (movementInput != Vector2.zero)
            lastDirection = movementInput;
    }

    private void HandleSprint()
    {
        bool canSprint =
            input.SprintHeld &&
            movementInput != Vector2.zero &&
            staminaSystem.CanSprint();

        staminaSystem.SetSprinting(canSprint);
    }

    private void Move()
    {
        rb.linearVelocity = movementInput * GetCurrentSpeed() * GetMovementMultiplier();
    }

    private float GetCurrentSpeed()
    {
        bool sprinting =
            input.SprintHeld &&
            movementInput != Vector2.zero &&
            staminaSystem.CanSprint();

        return sprinting ? sprintSpeed : walkSpeed;
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
        animator.SetFloat("moveX", lastDirection.x);
        animator.SetFloat("moveY", lastDirection.y);
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

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);

        knockbackRoutine = StartCoroutine(KnockbackRoutine(direction, force));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force)
    {
        isKnockedBack = true;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackRecoveryTime);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}