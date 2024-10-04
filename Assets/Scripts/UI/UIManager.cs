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
                
                HideItemTooltip();
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

        
        
        
        
        
        public GameObject HUD;
        public bool IsHideHUDOnScreenOpened;

        public GameObject
            ScreenPause,
            ScreenSettings,
            ScreenChat;
        
        
        
        public UIItemTooltip UiItemTooltip;

        public static void HideItemTooltip()
        {
            instance?.UiItemTooltip?.gameObject.SetActive(false);
        }

        public UIItemSlot UiItemHolding;

        public static void ShowItemHolding(ItemStack stack)
        {
            var h = instance.UiItemHolding;
            h.gameObject.SetActive(true);
            h.SetItemStack(stack);
        }
        
        public static void HideItemHolding()
        {
            instance?.UiItemHolding?.gameObject.SetActive(false);
        }

        
        
        public RectTransform UiCrosshairProgressBar;

        public void SetCrosshairProgress(float progress) {
            var parent = UiCrosshairProgressBar.parent as RectTransform;
            parent.gameObject.SetActive(progress > 0);
            if (progress <= 0) 
                return;
            UiCrosshairProgressBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (parent.sizeDelta.x-2) * progress);
        }


        private void Update()
        {
            if (UIItemSlot.holdingSlot) {
                UiItemHolding.transform.position = Input.mousePosition;
            }
            
            
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