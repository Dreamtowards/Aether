
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace Aether
{
    public class EntityPlayer : Entity
    {
        [ShowInInspector]
        public Inventory inventory = new(64);

    }
}