using System;
using UnityEngine;

namespace Aether
{
    public class SkyManager : MonoBehaviour
    {
        public Transform m_Sun;
        public Transform m_Moon;
        public float m_Yaw = -90;
        
        void Update()
        {
            var daytime = World.instance.DayTime;

            bool sun = daytime < 0.55f;
            m_Sun.gameObject.SetActive(sun);
            m_Moon.gameObject.SetActive(!sun);
            
            m_Sun.transform.rotation  = Quaternion.Euler(daytime * 360.0f, m_Yaw, 0);
            m_Moon.transform.rotation = Quaternion.Euler(daytime * 360.0f + 180.0f, m_Yaw, 0);

        }
    }
}