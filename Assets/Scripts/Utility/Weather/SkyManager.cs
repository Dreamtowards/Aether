using System;
using UnityEngine;

namespace Aether.Weather
{
    public class SkyManager : MonoBehaviour
    {
        public Transform m_Sun;
        public Transform m_Moon;
        
        void Update()
        {
            var daytime = World.instance.DayTime;

            m_Sun.gameObject.SetActive(daytime < 0.55f);
            m_Moon.gameObject.SetActive(daytime > 0.5f);
            
            m_Sun.transform.rotation  = Quaternion.Euler(daytime * 360.0f, -90, 0);
            m_Moon.transform.rotation = Quaternion.Euler(daytime * 360.0f + 180.0f, -90, 0);

        }
    }
}