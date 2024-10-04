using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aether
{
    public class UIItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Selectable m_Border;
        
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

        public bool IsLocked;
        
        public Image m_ItemImage;
        public Text m_CountText;

        [ShowInInspector]
        public ItemStack itemStack;

        [Button]
        public void UpdateItemStack()
        {
            var stack = itemStack;

            if (stack?.IsEmpty ?? true) {
                m_ItemImage.enabled = false;
                m_CountText.enabled = false;
            } else {
                m_ItemImage.enabled = true;
                m_CountText.enabled = true;
                
                m_ItemImage.sprite = stack.item.icon;
                m_CountText.text = stack.count.ToString();
            }
        }

        public void SetItemStack(ItemStack stack)
        {
            itemStack = stack;
            UpdateItemStack();
        }


        public void OnPointerEnter(PointerEventData evt)
        {
            if ((itemStack?.IsEmpty ?? true) || holdingSlot)
                return;
            var tip = UIManager.instance.UiItemTooltip;
            tip.gameObject.SetActive(true);
            tip.UpdateItemStack(itemStack);
            tip.transform.position = transform.position + tip.offset;
        }

        public void OnPointerExit(PointerEventData evt)
        {
            UIManager.HideItemTooltip();
        }

        public static UIItemSlot holdingSlot;

        public void SwapItem(UIItemSlot slot)
        {
            slot.itemStack.Swap(this.itemStack);
            this.UpdateItemStack();
            slot.UpdateItemStack();
        }
        
        public void OnPointerClick(PointerEventData evt)
        {
            if (IsLocked)
                return;

            if (holdingSlot)
            {
                SwapItem(holdingSlot);
                holdingSlot = null;
                UIManager.HideItemHolding();
            }
            else
            {
                if (this.itemStack?.IsEmpty == false)
                {
                    holdingSlot = this;
                    UIManager.HideItemTooltip();
                    UIManager.ShowItemHolding(holdingSlot.itemStack);
                }
            }
        }
    }
}