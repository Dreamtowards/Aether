using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public GameObject HUD;
        public bool IsHideHUDOnScreenOpened;

        public GameObject
            ScreenPause,
            ScreenSettings,
            ScreenChat;
        
        public UIItemTooltip UiItemTooltip;
        
        private void Start()
        {
            instance = this;
        }

        private static GameObject g_CurrentUI;
        public static GameObject CurrentScreen
        {
            get => g_CurrentUI;
            set
            {
                if (g_CurrentUI == value)
                    return;

                if (g_CurrentUI)
                    g_CurrentUI.SetActive(false);
				
                g_CurrentUI = value;
				
                if (g_CurrentUI)
                    g_CurrentUI.SetActive(true);
                
                InputManager.UpdateIsPlayingInput();
                
                // Hide HUD when opened UIScreen
                if (instance && instance.IsHideHUDOnScreenOpened)
                    instance.HUD?.SetActive(g_CurrentUI == null);
            }
        }

        private static List<GameObject> OpendScreens = new();

        public static void PushScreen(GameObject screen)
        {
            if (screen)
                OpendScreens.Add(screen);
            CurrentScreen = screen;
        }
        public static void PopScreen(bool keepLastScreen = false)
        {
            OpendScreens.RemoveIf(e => e == null);  // Clear Nil Screens after switch Scene
            
            if (OpendScreens.Count <= 1 && keepLastScreen)
                return;
            OpendScreens.TryRemoveLast();
            CurrentScreen = OpendScreens.LastOr(null);
        }

        // [Serializable]
        // public class Screens
        // {
        //     public GameObject
        //         MainTitle,
        //         WorldList,
        //         Settings,
        //         Pause,
        //         Chat;
        // }
        // [ShowInInspector]
        // public Screens m_Screens;
        //
        // public static Screens Screen => instance.m_Screens;

        // public GameObject
        //     UiScreenMainTitle,
        //     UiScreenWorldList,
        //     UiScreenSettings,
        //     UiScreenPause,
        //     UiScreenChat;
    }
}