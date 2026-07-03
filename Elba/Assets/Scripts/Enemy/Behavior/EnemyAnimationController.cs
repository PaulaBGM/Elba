using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private EnemyController controller;
    [SerializeField] private EnemyHealthSystem health;
    [SerializeField] private ChargeAnimalBehaviour chargeBehaviour;

    private Animator animator;

    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int IsStunned = Animator.StringToHash("isStunned");

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (controller == null)
            controller = GetComponentInParent<EnemyController>();

        if (health == null)
            health = GetComponentInParent<EnemyHealthSystem>();

        if (chargeBehaviour == null)
            chargeBehaviour = GetComponentInParent<ChargeAnimalBehaviour>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnEnemyDied += HandleDeath;

        if (chargeBehaviour != null)
        {
            chargeBehaviour.OnChargeStarted += PlayAttack;
            chargeBehaviour.OnStunnedStateChanged += HandleStunned;
        }
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnEnemyDied -= HandleDeath;

        if (chargeBehaviour != null)
        {
            chargeBehaviour.OnChargeStarted -= PlayAttack;
            chargeBehaviour.OnStunnedStateChanged -= HandleStunned;
        }
    }

    private void Update()
    {
        bool moving =
            controller != null &&
            controller.Rigidbody != null &&
            controller.Rigidbody.linearVelocity.sqrMagnitude > 0.01f;

        animator.SetBool(IsMoving, moving);
    }

    public void PlayAttack()
    {
        animator.SetTrigger(Attack);
    }

    private void HandleStunned(bool stunned)
    {
        animator.SetBool(IsStunned, stunned);
    }

    private void HandleDeath()
    {
        animator.SetBool(Dead, true);
    }

    // Este método lo llamas con un Animation Event dentro del clip de ataque
    public void DealDamage()
    {
        if (chargeBehaviour != null)
            chargeBehaviour.DealDamage();
    }
}