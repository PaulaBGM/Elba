using UnityEngine;

public class EnemyController : FSMController<EnemyController>
{
    [Header("References")]
    [field: SerializeField] public EnemySensor Sensor { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    public Transform Target { get; set; }

    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }

    private EnemyHealthSystem healthSystem;
    private EnemyLoot enemyLoot;

    [SerializeField] private float timeToDestroy = 2f;
    public bool IsBusy { get; set; }
    [SerializeField] private MonoBehaviour animalBehaviour;
    public IAnimalBehaviour AnimalBehaviour =>animalBehaviour as IAnimalBehaviour;
   
    private void Awake()
    {
        if (Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody2D>();

        healthSystem = GetComponentInParent<EnemyHealthSystem>();
        enemyLoot = GetComponent<EnemyLoot>();

        PatrolState = GetComponent<PatrolState>();
        ChaseState = GetComponent<ChaseState>();
        AttackState = GetComponent<AttackState>();

        PatrolState?.InitController(this);
        ChaseState?.InitController(this);
        AttackState?.InitController(this);

        SetState(PatrolState);
    }

    private void OnEnable()
    {
        if (healthSystem != null)
            healthSystem.OnEnemyDied += EnemyDead;
    }

    private void OnDisable()
    {
        if (healthSystem != null)
            healthSystem.OnEnemyDied -= EnemyDead;
    }

    private void EnemyDead()
    {
        enemyLoot?.DropLoot();

        if (Sensor != null)
            Sensor.enabled = false;

        if (PatrolState != null)
            PatrolState.enabled = false;

        if (ChaseState != null)
            ChaseState.enabled = false;

        if (AttackState != null)
            AttackState.enabled = false;

        if (Rigidbody != null)
            Rigidbody.linearVelocity = Vector2.zero;

        enabled = false;

        Destroy(gameObject, timeToDestroy);
    }
}