using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aether
{
    public class UIGrandButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData evt)
        {
            SoundManager.instance.PlaySfxButtonHover();
        }

        public void OnPointerClick(PointerEventData evt)
        {
            SoundManager.instance.PlaySfxButtonClick();
            
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}