using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aether
{
    public class UIManager : MonoBehaviour
    {
        public UIManager instance;

        private static GameObject g_CurrentUI;
        public static GameObject CurrentScreen
        {
            get => g_CurrentUI;
            set
            {
                if (g_CurrentUI == value)
                    return;
				
                InputManager.IsPlayingInput = value == null;
                Utility.LockCursor(InputManager.IsPlayingInput);

                g_CurrentUI?.SetActive(false);
				
                g_CurrentUI = value;
				
                g_CurrentUI?.SetActive(true);
            }
        }

        private static List<GameObject> OpendScreens = new();

        public static void PushScreen(GameObject screen)
        {
            OpendScreens.Add(screen);
            CurrentScreen = screen;
        }
        public static void PopScreen(bool keepLastScreen = false)
        {
            if (OpendScreens.Count == 1 && keepLastScreen)
                return;
            OpendScreens.TryRemoveLast();
            CurrentScreen = OpendScreens.LastOr(null);
        }
        
        private void Start()
        {
            instance = this;
        }
    }
}