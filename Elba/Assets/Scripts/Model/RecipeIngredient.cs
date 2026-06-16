using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [System.Serializable]
    public class RecipeIngredient
    {
        public ItemSO item;
        public int amount;
    }
}