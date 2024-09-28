using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aether
{
    public class UIGrandButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public AudioClip m_SFXHover;
        public AudioClip m_SFXClick;

        public Text m_TargetText;

        
        public void OnPointerEnter(PointerEventData evt)
        {
            SoundManager.instance.Play(m_SFXHover);
        }

        public void OnPointerClick(PointerEventData evt)
        {
            SoundManager.instance.Play(m_SFXClick);
            
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}