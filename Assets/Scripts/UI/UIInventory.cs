using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    public class UIInventory : MonoBehaviour
    {
        [ShowInInspector]
        public Inventory inventory;

        public GameObject m_PrefabItemSlot;
        public List<UIItemSlot> slots;

        [Button]
        public void UpdateInventory() {
            ClearAll();
            
            for (int i = 0; i < inventory.items.Count; i++) {
                var slot = Instantiate(m_PrefabItemSlot, transform).GetComponent<UIItemSlot>();
                slots.Add(slot);
                
                slot.m_ItemStack = inventory.items[i];
                slot.UpdateItemStack();
            }
        }

        [Button]
        public void ClearAll() {
            transform.DestroyChildren(true);
            slots.Clear();
        }
    }
}