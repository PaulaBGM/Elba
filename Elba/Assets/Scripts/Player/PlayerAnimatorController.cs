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
    private readonly int talando = Animator.StringToHash("Talando");
    private readonly int conObjeto = Animator.StringToHash("conObjeto");
    private readonly int isLow = Animator.StringToHash("isLow");

    private void Awake()
    {
        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (attack == null)
            attack = GetComponent<PlayerAttackSystem>();

        if (interaction == null)
            interaction = GetComponent<PlayerInteractionSystem>();

        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
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
        animator.SetBool(conObjeto, interaction.HasHeldItem);
    }

    private void HandleAttack(AttackType type)
    {
        Debug.Log($"Animación: {type}");

        switch (type)
        {
            case AttackType.Animal:
                animator.SetTrigger(isAttacking);
                break;

            case AttackType.Tree:
                animator.SetTrigger(talando);
                break;
        }
    }

    private void HandleGather(GatherType type)
    {
        animator.SetBool(isLow, type == GatherType.High);
        animator.SetTrigger(recoger);
    }

    public void EndGatherAnimation()
    {
        Debug.Log("Se agacho");
        animator.SetBool(isLow, false);
    }
    public void OnDeathAnimationFinished()
    {
        GameOverController.Instance?.Show();
    }

    private void HandleDeath()
    {
        animator.SetTrigger(isDying);
    }

    private void HandleTired(bool value)
    {
        animator.SetBool(isTired, value);
    }
}