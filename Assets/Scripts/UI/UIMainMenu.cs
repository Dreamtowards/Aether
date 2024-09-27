using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aether
{
    [Serializable]
    public class AudioInfo
    {
        public AudioClip clip;
        public float time;
    }
    
    public class UIMainMenu : MonoBehaviour
    {
        public float m_Time;
        
        public Light m_Light;
        public AnimationCurve m_LightIntensityCurve;
        
        public Camera m_Camera;
        public AnimationCurve m_CameraPitchCurve;
        public AnimationCurve m_CameraHeightCurve;
        
        [ShowInInspector]
        [SerializeField]
        public AudioInfo[] m_Musics;

        private void Start()
        {
            var aud = gameObject.AddComponent<AudioSource>();
            var bgm = m_Musics[Random.Range(0, m_Musics.Length)];
            aud.clip = bgm.clip;
            aud.time = bgm.time;
            aud.Play();
        }

        private void Update()
        {
            m_Time += Time.deltaTime;
            
            m_Light.intensity = m_LightIntensityCurve.Evaluate(m_Time);
            
            m_Camera.transform.rotation = Quaternion.Euler(m_CameraPitchCurve.Evaluate(m_Time), 0, 0);
            m_Camera.transform.position = m_CameraHeightCurve.Evaluate(m_Time) * Vector3.up;
        }
    }
}