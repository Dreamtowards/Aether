using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    using UInt8 = System.Byte;

    using Vec3 = Vector3;
    using IVec3 = Vector3Int;

    public static class Utility
    {
        public static int FrametimeIntervals(float interval, float time, float delta)
        {
            return (int)(math.floor(time / interval) - math.floor((time - delta) / interval));
        }

        public static bool AtInterval(float interval)
        {
            return FrametimeIntervals(interval, Time.deltaTime, Time.realtimeSinceStartup) != 0;
        }

        public static void ForVolume(int3 min, int3 max, Action<int3> visitor)
        {
            for (int x = min.x; x < max.x; x++)
                for (int y = min.y; y < max.y; y++)
                    for (int z = min.z; z < max.z; z++)
                        visitor(new int3(x, y, z));
        }

        public static int RemoveAll<K,V>(this Dictionary<K, V> dict, IEnumerable<K> removes)
        {
            int numRemoved = 0;
            foreach (var k in removes)
            {
                if (dict.Remove(k))
                    numRemoved++;
            }
            return numRemoved;
        }
    }
}