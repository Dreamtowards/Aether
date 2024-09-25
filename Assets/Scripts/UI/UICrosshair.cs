using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UICrosshair : MonoBehaviour
    {
        public Image m_CrosshairImage;

        void Update()
        {
            var hit = CursorRaycaster.instance.hitResult;

            m_CrosshairImage.color = hit.isHit ? Color.white : Color.white * 0.75f;
        }
    }
}