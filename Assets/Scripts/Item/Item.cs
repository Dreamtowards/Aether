using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Aether
{
    // NOTE: item prototype, handles logics instead of any data. only 1 instance for a type of item. if you want multiple items instances see ItemStack.
    [CreateAssetMenu(fileName = "Data", menuName = "Aether/Item")]
    public class Item : SerializedScriptableObject
    {
        public string id;
        public string name;
        public Sprite icon;
        public string description;

        public int maxCount = 64;
        
        public Rarity rarity = Rarity.Common;

        public float fuel;
        
        // [NonSerialized, OdinSerialize]
        public List<ItemComponent> components = new();

        public void OnUse(EntityPlayer user, ItemStack stack)
        {
            components.ForEach(e => e.OnUse(user, stack));
        }

        public void OnUsingTick() { }

        public void OnUseCompleted(EntityPlayer user, ItemStack stack)
        {
            components.ForEach(e => e.OnUseCompleted(user, stack));
        }

        public float GetMaxUseTime() {
            float maxTime = 0;
            foreach (var c in components) {
                maxTime += c.GetMaxUseTime();
            }
            return maxTime;
        }
        
        public bool IsItemBarVisible(ItemStack stack) => false;

        public float GetItemBarStep(ItemStack stack) => 1;

        // public float GetMiningSpeed(ItemStack stack, Vox vox) {
        //     if (!stack.GetItemComponent<ItemComponentTool>(out var tool))
        //         return 1;
        //     return tool.GetMiningSpeed(vox);
        // }



        public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }

    }
    
    public class ItemComponent
    {
        // on starts using. Right Click
        public virtual void OnUse(EntityPlayer user, ItemStack stack)
        {
            
        }
        
        public virtual void OnUseCompleted(EntityPlayer user, ItemStack stack)
        {
            
        }

        // the maximum use (right-click) time of this item
        public virtual float GetMaxUseTime() {
            return 0;
        }
    }

    public class ItemComponentFood : ItemComponent
    {
        public int heal;
        public float eatTime = 2;

        public override void OnUseCompleted(EntityPlayer user, ItemStack stack)
        {
            user.health += heal;
            stack.Decrement();
        }

        public override float GetMaxUseTime() => eatTime;
    }
        
    public class ItemComponentTool : ItemComponent
    {
        public int durability = 10;
        
    }
}