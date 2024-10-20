﻿using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Aether
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [NonSerialized] 
        public ObjectPool<AudioSource> m_Pool;
        
        [NonSerialized] 
        public List<AudioSource> m_Playing = new();
        
        public AudioClip m_SFXHover;
        public AudioClip m_SFXClick;

        private void Start()
        {
            instance = this;
            
            m_Pool = new(() => gameObject.AddComponent<AudioSource>());
        }

        private void FixedUpdate()
        {
            ReleaseAllStopped();
        }

        public AudioSource Play(AudioClip clip, float time = 0)
        {
            var aud = m_Pool.Get();
            aud.clip = clip;
            aud.time = time;
            aud.Play();
            m_Playing.Add(aud);
            return aud;
        }

        public void PlaySfxButtonHover() {
            Play(m_SFXHover);
        }
        public void PlaySfxButtonClick() {
            Play(m_SFXClick);
        }
        
        private void ReleaseAllStopped()
        {
            m_Playing.RemoveIf(e => {
                if (!e.isPlaying) {
                    m_Pool.Release(e);
                    return true;
                }
                return false;
            });
        }

        public static AudioSource PlaySelf(Transform trans, AudioClip clip) {
            var aud = trans.GetOrAddComponent<AudioSource>();
            aud.clip = clip;
            aud.Play();
            return aud;
        }
    }
}