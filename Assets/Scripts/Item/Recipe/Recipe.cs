using System;
using System.Collections.Generic;

namespace Aether
{
    [Serializable]
    public class Recipe
    {
        public ItemStack output;

        public List<ItemStack> inputs = new();
    }
}