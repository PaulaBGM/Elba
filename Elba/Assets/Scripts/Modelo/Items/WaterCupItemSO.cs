using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Water Cup")]
    public class WaterCupItemSO : EdibleItemSO
    {
        [SerializeField] private CupItemSO emptyCup;

        public override bool PerformAction(
            GameObject character,
            List<ItemParameter> itemState = null)
        {
            bool result =
                base.PerformAction(character, itemState);

            if (!result)
                return false;

            InventoryController inventory =
                character.GetComponent<InventoryController>();

            inventory.InventoryData.AddItem(
                emptyCup,
                1);

            return true;
        }
    }
}