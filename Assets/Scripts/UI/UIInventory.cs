using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    public class UIInventory : MonoBehaviour
    {
        [ShowInInspector]
        public Inventory m_Inventory;

        public GameObject m_PrefabItemSlot;
        public List<UIItemSlot> m_Slots;
        
        [Button]
        public void Realloc(int size) 
        {
            transform.DestroyChildren(true);
            
            m_Slots.Clear();
            for (int i = 0; i < size; i++)
            {
                var slot = Instantiate(m_PrefabItemSlot, transform).GetComponent<UIItemSlot>();
                m_Slots.Add(slot);
            }
        }
    }
}