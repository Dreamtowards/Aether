using System;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIHotbar : MonoBehaviour
    {
        public int CurrentSlotIndex;
        public int SlotSize = 9;
        
        
        
        void Update()
        {
            CurrentSlotIndex += (int)Mathf.Sign(Input.mouseScrollDelta.y);
            CurrentSlotIndex %= SlotSize;
            //Maths.Mod(CurrentSlotIndex, SlotSize);

            

        }
    }
}