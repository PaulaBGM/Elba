using System;

namespace Inventory.Model
{
    [Flags]
    public enum ItemCategory
    {
        None = 0,

        Food = 1 << 0,
        Tool = 1 << 1,
        Material = 1 << 2,

        All = Food | Tool | Material
    }
}