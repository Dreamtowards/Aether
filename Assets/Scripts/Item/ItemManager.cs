using System;
using UnityEngine;

namespace Aether
{
    public class ItemManager : MonoBehaviour
    {
        public ItemRegistry registry;
        
        public static ItemManager instance;

        private void Awake()
        {
            instance = this;
        }
    }
}