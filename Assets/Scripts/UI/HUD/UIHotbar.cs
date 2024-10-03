using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIHotbar : MonoBehaviour
    {
        // public int CurrentSlotIndex = -1;  // -1 so triggers SlotChange, apply selected
        public int SlotSize = 9;
        
        public UIItemSlot[] m_ItemSlots;

        public Image m_HealthBar;
        public int m_HealthBarMaxWidth = 236;
        
        private void Start()
        {
            m_ItemSlots = transform.GetComponentsInChildren<UIItemSlot>();
        }

        void Update()
        {
            var player = InputManager.instance.player;
            
            UpdateHealthBar(player.health / player.maxHealth);
            
            UpdateHotbarSelection(player);
            
            UpdateHotbarSlots(player);
        }

        public void UpdateHealthBar(float healthPercent)
        {
            m_HealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthPercent * m_HealthBarMaxWidth);
        }

        private void UpdateHotbarSelection(EntityPlayer player)
        {
            var oldSlot = player.holdingSlotIndex;
            var newSlot = oldSlot;
            
            if (InputManager.IsPlayingInput && !InputManager.instance.actionCameraDistanceModifier.IsPressed()) 
                newSlot += (int)math.sign(-Input.mouseScrollDelta.y);
            newSlot = (int)Maths.Mod(newSlot, SlotSize);

            if (oldSlot != newSlot) 
            {
                m_ItemSlots[oldSlot].IsSelected = false;
                m_ItemSlots[newSlot].IsSelected = true;
                player.holdingSlotIndex = newSlot;
            } 
            else if (!m_ItemSlots[newSlot].IsSelected)
            {
                m_ItemSlots[newSlot].IsSelected = true;
            }
        }

        private void UpdateHotbarSlots(EntityPlayer player)
        {
            for (int i = 0; i < m_ItemSlots.Length; i++) {
                var slot = m_ItemSlots[i];
                slot.itemStack = player.inventory.items[i];
                slot.UpdateItemStack();
            }
        }
    }
}