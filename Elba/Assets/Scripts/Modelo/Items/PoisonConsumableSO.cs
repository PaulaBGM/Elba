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

        public override bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            base.PerformAction(character, itemState);

            PlayerPoisonSystem poison = character.GetComponent<PlayerPoisonSystem>();

            if (poison != null)
            {
                poison.ApplyPoison(poisonDamage,poisonDuration);
            }
            return true;
        }
    }
}