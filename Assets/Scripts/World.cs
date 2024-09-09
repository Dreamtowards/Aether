using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aether
{
    public class World : MonoBehaviour
    {
        [SerializeField]
        private WorldInfo m_WorldInfo = new WorldInfo();

        public List<EntityPlayer> m_Players = new();

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        public UInt32 Seed()
        {
            return m_WorldInfo.Seed;
        }
        public float DayTime()
        {
            return m_WorldInfo.Seed;
        }
    }

    [Serializable]
    public class WorldInfo
    {
        public string Name;
        public UInt32 Seed;
        
        public float DayTime;
        public float DayTimeLength;

        public UInt64 TimeCreated;
        public UInt64 TimeMidified;
    }
}