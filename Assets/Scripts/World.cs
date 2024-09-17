using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Aether
{
    public class World : MonoBehaviour
    {
        [SerializeField]
        private WorldInfo m_WorldInfo = new WorldInfo();

        public List<EntityPlayer> m_Players = new();

        public static World instance;

        void Start()
        {
            Assert.IsNull(instance);
            instance = this;

        }
        
        void Update()
        {
            ref var wi = ref m_WorldInfo;
            if (wi.DayTimeLength != 0)
                wi.DayTime += Time.deltaTime / wi.DayTimeLength;
            wi.DayTime = math.frac(wi.DayTime);

            
        }

        public WorldInfo Info => m_WorldInfo;
        public UInt32 Seed => m_WorldInfo.Seed;
        public float DayTime => m_WorldInfo.DayTime;
    }

    [Serializable]
    public class WorldInfo
    {
        public string Name;
        public UInt32 Seed;
        
        public float DayTime;  // [0, 1]. 0=6AM, 0.5=6PM
        public float DayTimeLength = 60 * 60;  // Seconds

        public UInt64 TimeCreated;
        public UInt64 TimeMidified;
        
        public float TimeInhabited;
    }
}