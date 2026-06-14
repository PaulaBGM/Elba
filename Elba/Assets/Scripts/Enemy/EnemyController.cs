using UnityEngine;

public class EnemyController : FSMController<EnemyController>, IDamageable
{
    [Header("References")]
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [field: SerializeField] public EnemySensor Sensor { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;

    [Header("States")]
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ChaseState chaseState;
    [SerializeField] private AttackState attackState;

    [Header("Behaviour")]
    [SerializeField] private MonoBehaviour animalBehaviour;

    private IAnimalBehaviour behaviour;
    private float currentHealth;

    public Transform Target { get; set; }

    public PatrolState PatrolState => patrolState;
    public ChaseState ChaseState => chaseState;
    public AttackState AttackState => attackState;

    public IAnimalBehaviour AnimalBehaviour => behaviour;

    private void Awake()
    {
        currentHealth = maxHealth;

        behaviour = animalBehaviour as IAnimalBehaviour;

        patrolState.InitController(this);
        chaseState.InitController(this);
        attackState.InitController(this);
    }

    private void Start()
    {
        SetState(patrolState);
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}