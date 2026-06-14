using UnityEngine;

public interface IAnimalBehaviour
{
    void Attack(EnemyController enemy);
    bool IsBusy { get; }
}