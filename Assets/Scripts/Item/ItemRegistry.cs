using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    [CreateAssetMenu(fileName = "Data", menuName = "Aether/Item Registry")]
    public class ItemRegistry : SerializedScriptableObject
    {
        public List<Item> Entries;
        
    }
}