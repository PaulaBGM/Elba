using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Structure")]
    public class StructureItemSO : ItemSO
    {
        [field: SerializeField]
        public GameObject PlaceablePrefab { get; private set; }

        public bool IsPlaceable => PlaceablePrefab != null;
    }
}