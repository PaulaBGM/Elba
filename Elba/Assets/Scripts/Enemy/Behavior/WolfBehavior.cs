using UnityEngine;

public class WolfBehaviour : MonoBehaviour, IAnimalBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float grabDuration = 2f;

    private float timer;

    public bool IsBusy => timer > 0f;

    private void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
    }

    public void Attack(EnemyController enemy)
    {
        if (enemy.Target == null)
            return;

        PlayerMain player =
            enemy.Target.GetComponent<PlayerMain>();

        if (player == null)
            return;

        player.Damage(damage);

        timer = grabDuration;
    }
}