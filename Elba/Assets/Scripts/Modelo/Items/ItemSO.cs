using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{

    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }

        [field: SerializeField]
        public string ItemID { get; private set; }

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }
        
        [field: SerializeField]
        public GameObject WorldPrefab { get; private set; }
        
        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }

        [field: SerializeField]
        public ItemCategory Categories { get; private set; }
       
        [Header("Cooking")]
        public bool canCook;
        public ItemSO cookedResult;

    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
