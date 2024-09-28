using System;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIHitInfo : MonoBehaviour
    {
        public GameObject m_UiPanel;
        
        public Text m_Title;
        
        void Update()
        {
            var hit = CursorRaycaster.instance.hitResult;

            m_UiPanel.SetActive(hit.isHit);
            if (hit.isHit)
            {
                m_Title.text = hit.hitInfo.transform.name;
            }
        }
    }
}