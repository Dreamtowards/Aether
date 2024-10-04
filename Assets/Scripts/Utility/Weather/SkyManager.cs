using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Aether
{
    public class SkyManager : MonoBehaviour
    {
        public Light m_Sun;
        public Transform m_Moon;
        public float m_Yaw = -90;
        
        public LensFlareComponentSRP m_LensFlare;
        
        public AnimationCurve m_SunIntensityCurve;
        
        void Update()
        {
            if (!Utility.AtInterval(0.5f))
                return;
            
            var daytime = World.instance.DayTime;
            m_LensFlare.enabled = daytime < 0.5f;
            
            bool sun = daytime < 0.52f || daytime > 0.98f;
            m_Sun.gameObject.SetActive(sun);
            m_Moon.gameObject.SetActive(!sun);
            
            m_Sun.transform.rotation  = Quaternion.Euler(daytime * 360.0f, m_Yaw, 0);
            m_Moon.transform.rotation = Quaternion.Euler(daytime * 360.0f + 180.0f, m_Yaw, 0);

            m_Sun.intensity = m_SunIntensityCurve.Evaluate(daytime);
        }
    }
}