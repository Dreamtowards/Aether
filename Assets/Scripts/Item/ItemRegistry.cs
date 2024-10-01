using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Aether
{
    [CreateAssetMenu(fileName = "Data", menuName = "Aether/Item Registry")]
    public class ItemRegistry : SerializedScriptableObject
    {
        public List<Item> entries = new();
        
    }
}