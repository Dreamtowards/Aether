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
    }
}