using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIHotbar : MonoBehaviour
    {
        public int CurrentSlotIndex = -1;  // -1 so triggers SlotChange, apply selected
        public int SlotSize = 9;
        
        public UIItemSlot[] m_ItemSlots;

        public Image m_HealthBar;
        public int m_HealthBarMaxWidth = 236;
        
        public EntityPlayer m_EntityPlayer;

        private void Start()
        {
            m_ItemSlots = transform.GetComponentsInChildren<UIItemSlot>();
        }

        void Update()
        {
            UpdateHealthBar(m_EntityPlayer.health / m_EntityPlayer.maxHealth);
            
            UpdateHotbarSelection();
            
            UpdateHotbarSlots();
        }

        public void UpdateHealthBar(float healthPercent)
        {
            m_HealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthPercent * m_HealthBarMaxWidth);
        }

        private void UpdateHotbarSelection()
        {
            var oldSlot = CurrentSlotIndex;
            
            if (!InputManager.instance.actionCameraDistanceModifier.IsPressed())
                CurrentSlotIndex += (int)math.sign(-Input.mouseScrollDelta.y);
            CurrentSlotIndex = (int)Maths.Mod(CurrentSlotIndex, SlotSize);

            if (oldSlot != CurrentSlotIndex) {
                m_ItemSlots[oldSlot].IsSelected = false;
                m_ItemSlots[CurrentSlotIndex].IsSelected = true;
            }
        }

        private void UpdateHotbarSlots()
        {
            for (int i = 0; i < m_ItemSlots.Length; i++) {
                var slot = m_ItemSlots[i];
                slot.ItemStack = m_EntityPlayer.inventory.items[i];
                slot.UpdateItemStack();
            }
        }
    }
}