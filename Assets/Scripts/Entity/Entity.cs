
using UnityEngine;

namespace Aether
{
    public class Entity : MonoBehaviour
    {
        public string Name;


        // whether the entity can be attacked by players.
        public bool IsAttackable()
        {
            return true;
        }

        public void Interact(EntityPlayer player, Vector3 pos)
        {
            
        }
    }
}