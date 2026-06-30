using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttackSystem attack;
    [SerializeField] private PlayerInteractionSystem interaction;
    [SerializeField] private PlayerStatsSystem stats;
    [SerializeField] private Animator animator;
    private readonly int moveX = Animator.StringToHash("moveX");
    private readonly int moveY = Animator.StringToHash("moveY");
    private readonly int isMoving = Animator.StringToHash("isMoving");
    private readonly int isAttacking = Animator.StringToHash("isAttacking");
    private readonly int isDying = Animator.StringToHash("isDying");
    private readonly int isTired = Animator.StringToHash("isTired");
    private readonly int recoger = Animator.StringToHash("Recoger");

    private bool lastMoving;

    private void Awake()
    {
        Debug.Log($"Animator encontrado: {animator}");

        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (attack == null)
            attack = GetComponent<PlayerAttackSystem>();

        if (interaction == null)
            interaction = GetComponent<PlayerInteractionSystem>();

        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();

        Debug.Log($"Movement: {movement}");
        Debug.Log($"Attack: {attack}");
        Debug.Log($"Interaction: {interaction}");
        Debug.Log($"Stats: {stats}");
    }

    private void OnEnable()
    {
        Debug.Log("Suscribiendo eventos");

        attack.OnAttackStarted += HandleAttack;
        interaction.OnGatherStarted += HandleGather;
        stats.OnDeath += HandleDeath;
        stats.OnTiredStateChanged += HandleTired;
    }

    private void OnDisable()
    {
        attack.OnAttackStarted -= HandleAttack;
        interaction.OnGatherStarted -= HandleGather;
        stats.OnDeath -= HandleDeath;
        stats.OnTiredStateChanged -= HandleTired;
    }

    private void Update()
    {
        animator.SetFloat(moveX, movement.LastDirection.x);
        animator.SetFloat(moveY, movement.LastDirection.y);
        animator.SetBool(isMoving, movement.IsMoving);

        if (movement.IsMoving != lastMoving)
        {
            lastMoving = movement.IsMoving;

            Debug.Log($"isMoving = {movement.IsMoving}");
            Debug.Log($"moveX = {movement.LastDirection.x}");
            Debug.Log($"moveY = {movement.LastDirection.y}");
        }
    }

    private void HandleAttack()
    {
        Debug.Log("ATTACK Trigger");

        animator.SetTrigger(isAttacking);
    }

    private void HandleGather()
    {
        Debug.Log("GATHER Trigger");

        animator.SetTrigger(recoger);
    }

    private void HandleDeath()
    {
        Debug.Log("DEATH");

        animator.SetBool(isDying, true);
    }

    private void HandleTired(bool value)
    {
        Debug.Log($"TIRED = {value}");

        animator.SetBool(isTired, value);
    }
}