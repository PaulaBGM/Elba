using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equipar";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(
            GameObject character,
            List<ItemParameter> itemState = null)
        {
            AgentWeapon weaponSystem =
                character.GetComponent<AgentWeapon>();

            if (weaponSystem == null)
                return false;

            weaponSystem.SetWeapon(
                this,
                itemState ?? DefaultParametersList);

            return true;
        }
    }
}