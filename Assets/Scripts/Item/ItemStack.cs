namespace Aether
{
    public struct ItemStack
    {
        public Item item;
        public int count;

        public ItemStack(Item item, int count) {
            this.item = item;
            this.count = count;
            this.m_Damage = 0;
        }


        public bool IsEmpty => count == 0 || item == null;

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