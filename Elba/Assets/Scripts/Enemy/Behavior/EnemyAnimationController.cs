using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private EnemyController controller;
    [SerializeField] private EnemyHealthSystem health;

    private Animator animator;

    static readonly int IsMoving = Animator.StringToHash("isMoving");
    static readonly int Attack = Animator.StringToHash("Attack");
    static readonly int Dead = Animator.StringToHash("Dead");

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (controller == null)
            controller = GetComponentInParent<EnemyController>();

        if (health == null)
            health = GetComponentInParent<EnemyHealthSystem>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnEnemyDied += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnEnemyDied -= HandleDeath;
    }

    private void Update()
    {
        bool moving =
            controller.Rigidbody.linearVelocity.sqrMagnitude > 0.01f;

        animator.SetBool(IsMoving, moving);
    }

    public void PlayAttack()
    {
        animator.SetTrigger(Attack);
    }

    private void HandleDeath()
    {
        animator.SetBool(Dead, true);
    }
}