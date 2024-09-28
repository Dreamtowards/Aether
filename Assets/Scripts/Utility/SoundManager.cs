using System;
using UnityEngine;

namespace Aether
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        private void Start()
        {
            instance = this;
            
        }

        public void Play(AudioClip clip)
        {
            
        }
    }
}