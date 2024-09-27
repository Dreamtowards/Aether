using System;
using UnityEngine;

namespace Aether
{
    public class UIMainMenu : MonoBehaviour
    {
        public float m_Time;
        
        public Light m_Light;
        public AnimationCurve m_LightIntensityCurve;
        
        public Camera m_Camera;
        public AnimationCurve m_CameraPitchCurve;
        public AnimationCurve m_CameraHeightCurve;
        
        private void Update()
        {
            m_Time += Time.deltaTime;
            
            m_Light.intensity = m_LightIntensityCurve.Evaluate(m_Time);
            
            m_Camera.transform.rotation = Quaternion.Euler(m_CameraPitchCurve.Evaluate(m_Time), 0, 0);
            m_Camera.transform.position = m_CameraHeightCurve.Evaluate(m_Time) * Vector3.up;
        }
    }
}