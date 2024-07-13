using System;
using UnityEngine;

namespace Aether
{
    public class World : MonoBehaviour
    {
        private WorldInfo m_WorldInfo = new WorldInfo();

        void Start()
        {
            
        }

        void Update()
        {
            
        }
    }


    public struct WorldInfo
    {
        public string Name;
        public UInt32 Seed;
        
        public float DayTime;
        public float DayTimeLength;

        public UInt64 TimeCreated;
        public UInt64 TimeMidified;
    }
}