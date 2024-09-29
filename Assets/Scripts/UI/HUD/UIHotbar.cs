using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIHotbar : MonoBehaviour
    {
        public int CurrentSlotIndex;
        public int SlotSize = 9;
        
        public UIItemSlot[] m_ItemSlots;

        private void Start()
        {
            m_ItemSlots = transform.GetComponentsInChildren<UIItemSlot>();
        }

        void Update()
        {
            var oldSlot = CurrentSlotIndex;
            
            CurrentSlotIndex += (int)math.sign(-Input.mouseScrollDelta.y);
            CurrentSlotIndex = (int)Maths.Mod(CurrentSlotIndex, SlotSize);

            if (oldSlot != CurrentSlotIndex)
            {
                m_ItemSlots[oldSlot].IsSelected = false;
                m_ItemSlots[CurrentSlotIndex].IsSelected = true;
            }
        }
    }
}