
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace Aether
{
    public class EntityPlayer : Entity
    {
        [ShowInInspector]
        public Inventory inventory = new(64);

        public float health = 100;
        public float maxHealth = 100;

        public PlayerAbilities abilities = new();
        
        public bool isSprinting;
        
        public int holdingSlotIndex;
        public bool isUsingItem;

        public ItemStack GetHoldingItem()
        {
            return inventory.items[holdingSlotIndex];
        }

        public void DropHoldingItem(bool full = false)
        {
            
        }

        public void Attack(Entity target) {
            if (!target.IsAttackable())
                return;
            
        }

        public class PlayerAbilities
        {
            public bool
                invulnerable,
                flying,
                allowFlying,
                creativeMode,
                allowModifyWorld = true;
            
            public float 
                flySpeed = 0.05f,
                walkSpeed = 0.1f;
        }
    }
}