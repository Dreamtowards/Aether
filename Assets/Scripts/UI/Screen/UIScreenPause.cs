using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aether
{
    public class UIScreenPause : MonoBehaviour
    {
        public Button 
            m_BtnSettings,
            m_BtnQuit;

        public GameObject m_ScreenSettings;
        
        private void Start()
        {
            m_BtnSettings.onClick.AddListener(() =>
            {
                UIManager.PushScreen(m_ScreenSettings);
            });
            m_BtnQuit.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scenes/Limbo");
            });
        }
        
        public UIInventory m_UIInventory;
        public EntityPlayer m_Player;
        
        void OnEnable()
        {
            m_UIInventory.inventory = m_Player.inventory;
            m_UIInventory.UpdateInventory();
        }
    }
}