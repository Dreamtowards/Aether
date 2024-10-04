using System;

namespace Aether
{
    [Serializable]
    public class ItemStack
    {
        public Item item;
        public int count;

        public ItemStack(Item item = null, int count = 1) {
            this.item = item;
            this.count = count;
        }

        public void Swap(ItemStack other)
        {
            (this.item, other.item) = (other.item, this.item);
            (this.count, other.count) = (other.count, this.count);
        }

        public bool IsEmpty => count == 0 || item == null;

        public void Increment(int n = 1) {
            count += n;
        }
        public void Decrement(int n = 1) {
            if (count == 0)
                return;
            count -= n;
        }

        #region Damage

        private int m_Damage;

        public bool IsDamageable() => false;
        // return this.contains(DataComponentTypes.MAX_DAMAGE) && !this.contains(DataComponentTypes.UNBREAKABLE) && this.contains(DataComponentTypes.DAMAGE);

        public bool IsDamaged() => false;

        public int GetDamage() => 0;

        public int GetMaxDamage() => 0;
        
        public void SetDamage(int damage) {}
        

        public bool Damage(int amount, EntityPlayer player) {
            if (!IsDamageable())  // || player.IsGamemodeCreative)
                return false;
            
            int i = GetDamage() + amount;
            SetDamage(i);
            if (i >= GetMaxDamage()) {
                // CountSub(1);
                return true;
            }
            return false;
        }
        

        #endregion
        
        
    }
}