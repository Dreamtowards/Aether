using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Aether
{
    // NOTE: item prototype, handles logics instead of any data. only 1 instance for a type of item. if you want multiple items instances see ItemStack.
    [Serializable]
    public class Item
    {
        public string id;
        public string name;
        public Sprite icon;

        public int maxCount = 1;
        
        public Rarity rarity = Rarity.Common;

        public float fuel;
        
        [NonSerialized, OdinSerialize]
        [ShowInInspector]
        public List<ItemComponent> components = new();

        // on starts using. Right Click
        public void OnUse(EntityPlayer user, ItemStack stack)
        {
            
        }

        public void OnUsingTick() { }

        public void OnUseFinished() { }
        
        public void OnUseInterrupted() {}

        public bool IsItemBarVisible(ItemStack stack) => false;

        public float GetItemBarStep(ItemStack stack) => 1;

        // public float GetMiningSpeed(ItemStack stack, Vox vox) {
        //     if (!stack.GetItemComponent<ItemComponentTool>(out var tool))
        //         return 1;
        //     return tool.GetMiningSpeed(vox);
        // }



        public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }

    }
    
    [Serializable]
    public class ItemComponent
    {
        public int tmp = 2;
    }

    [Serializable]
    public class ItemComponentFood : ItemComponent
    {
        public int heal;
    }
        
    [Serializable]
    public class ItemComponentTool : ItemComponent
    {
        public int durability = 10;
        
    }
}