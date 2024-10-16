﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIMainMenu : MonoBehaviour
    {
        public Button 
            m_BtnPlay, 
            m_BtnSettings, 
            m_BtnExit, 
            m_BtnCredits;

        public GameObject 
            m_ScreenPlay, 
            m_ScreenSettings, 
            m_ScreenCredits;
        
        private void Start()
        {
            UIManager.PushScreen(gameObject);  // so switch to other screen will hide this screen.
            
            m_BtnPlay.onClick.AddListener(() =>
            {
                UIManager.PushScreen(m_ScreenPlay);
            });
            
            m_BtnSettings.onClick.AddListener(() =>
            {
                UIManager.PushScreen(m_ScreenSettings);
            });
            
            m_BtnExit.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            
            m_BtnCredits.onClick.AddListener(() =>
            {
                UIManager.PushScreen(m_ScreenCredits);
            });
        }
    }
}