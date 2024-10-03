using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Aether
{
    public class Inventory
    {
        public List<ItemStack> items = new();

        public Inventory() {
        }
        public Inventory(int size) {
            if (size > 0)
                Resize(size);
        }

        [Button]
        public void Resize(int size) {
            items.Clear();
            for (int i = 0; i < size; i++) {
                items.Add(new());
            }
        }
    }
}