
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

    }
}