using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Aether
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [NonSerialized] 
        public ObjectPool<AudioSource> m_Pool;

        private void Start()
        {
            instance = this;
            
            m_Pool = new(() => gameObject.AddComponent<AudioSource>());
        }

        public void Play(AudioClip clip)
        {
            var aud = m_Pool.Get();
            aud.clip = clip;
            aud.Play();
        }
    }
}