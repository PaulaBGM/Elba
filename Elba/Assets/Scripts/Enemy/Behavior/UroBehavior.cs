using UnityEngine;

public class UroBehaviour : MonoBehaviour, IAnimalBehaviour
{
    [SerializeField] private float damage = 40f;

    public void Attack(EnemyController enemy)
    {
        if (enemy.Target == null)
            return;

        PlayerMain player =
            enemy.Target.GetComponent<PlayerMain>();

        if (player == null)
            return;

        player.Damage(damage);
    }
}