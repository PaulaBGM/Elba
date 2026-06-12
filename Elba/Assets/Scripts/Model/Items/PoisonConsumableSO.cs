using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Poison Consumable")]
    public class PoisonConsumableSO : EdibleItemSO
    {
        [Header("Poison")]
        [SerializeField] private float poisonDamage;
        [SerializeField] private float poisonDuration;

        public float PoisonDamage => poisonDamage;
        public float PoisonDuration => poisonDuration;
    }
}