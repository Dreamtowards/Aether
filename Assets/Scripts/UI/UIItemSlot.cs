using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIItemSlot : MonoBehaviour
    {
        public Button m_Border;
        
        private bool m_IsSelected;

        public bool IsSelected {
            get => m_IsSelected;
            set
            {
                var col = m_Border.colors;
                col.normalColor = value ? Color.white : Color.clear;
                m_Border.colors = col;
                m_IsSelected = value;
            }
        }
        
        public Image m_ItemImage;
        public Text m_CountText;

        [ShowInInspector]
        public ItemStack m_ItemStack;

        [Button]
        public void UpdateItemStack()
        {
            var stack = m_ItemStack;

            if (stack.IsEmpty) {
                m_ItemImage.enabled = false;
                m_CountText.enabled = false;
            } else {
                m_ItemImage.enabled = true;
                m_CountText.enabled = true;
                
                m_ItemImage.sprite = stack.item.icon;
                m_CountText.text = stack.count.ToString();
            }
        }
        
    }
}