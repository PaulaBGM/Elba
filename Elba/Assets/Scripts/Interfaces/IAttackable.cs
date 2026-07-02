using UnityEngine;

public interface IAttackable
{
    void ReceiveHit(GameObject attacker, float damage);
}