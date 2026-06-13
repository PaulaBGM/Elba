using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Tool")]
    public class ToolItemSO : EquippableItemSO
    {
        [field: SerializeField]
        public ToolType ToolType { get; private set; }
    }
}