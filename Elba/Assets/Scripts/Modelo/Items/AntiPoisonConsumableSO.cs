using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Anti Poison Food")]
    public class AntiPoisonConsumableSO : EdibleItemSO
    {
        public override bool PerformAction(GameObject character,List<ItemParameter> itemState = null)
        {
            base.PerformAction(character, itemState);
            PlayerPoisonSystem poison = character.GetComponent<PlayerPoisonSystem>();poison?.CurePoison();

            return true;
        }
    }
}