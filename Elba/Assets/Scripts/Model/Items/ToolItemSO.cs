using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Tool")]
    public class ToolItemSO : EquippableItemSO
    {
        [field: SerializeField]
        public ToolType ToolType { get; private set; }

        [field: SerializeField]
        public int ResourceDamage { get; private set; } = 1;

        [field: SerializeField]
        public float AnimalDamage { get; private set; } = 10f;

        [field: SerializeField]
        public int DurabilityCost { get; private set; } = 1;
    }
}