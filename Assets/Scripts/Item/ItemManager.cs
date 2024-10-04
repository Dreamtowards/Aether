using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aether
{
    public class ItemManager : MonoBehaviour
    {
        public ItemRegistry registry;
        
        public RecipeRegistry recipes;
        
        public static ItemManager instance;

        private void Awake()
        {
            instance = this;
        }
    }
}