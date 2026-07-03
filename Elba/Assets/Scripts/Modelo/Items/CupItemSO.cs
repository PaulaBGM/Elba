using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Inventory/Items/Cup")]
    public class CupItemSO : EquippableItemSO
    {
        [Header("Sprites")]
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite fullSprite;
        public Sprite FullSprite => fullSprite;
        [Header("Gameplay")]
        [SerializeField] private float thirstRecovered = 40f;

        private bool isFilled;

        public bool IsFilled => isFilled;

        public float ThirstRecovered => thirstRecovered;

        public Sprite CurrentSprite =>
            isFilled ? fullSprite : emptySprite;

        public void Fill()
        {
            if (isFilled)
            {
                Debug.Log("[Cup] El vaso ya estaba lleno.");
                return;
            }

            isFilled = true;

            Debug.Log("[Cup] Vaso llenado.");
        }

        public bool Drink(GameObject player)
        {
            if (!isFilled)
            {
                Debug.Log("[Cup] El vaso está vacío.");
                return false;
            }

            PlayerStatsSystem stats =
                player.GetComponent<PlayerStatsSystem>();

            if (stats != null)
            {
                stats.ModifyStat(
                    StatType.Thirst,
                    thirstRecovered);
            }

            isFilled = false;

            Debug.Log("[Cup] El jugador ha bebido agua.");

            return true;
        }
    }
}