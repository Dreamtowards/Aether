using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    
    public static class Utility
    {
        public static int FrametimeIntervals(float interval, float time, float delta)
        {
            return (int)(math.floor(time / interval) - math.floor((time - delta) / interval));
        }

        public static bool AtInterval(float interval)
        {
            return FrametimeIntervals(interval, Time.deltaTime, Time.time) != 0;
        }

        public static void ForVolume(int3 min, int3 max, Action<int3> visitor)
        {
            for (int x = min.x; x < max.x; x++)
            for (int y = min.y; y < max.y; y++)
            for (int z = min.z; z < max.z; z++)
                visitor(new int3(x, y, z));
        }

        public static void ForVolumeSpread(int nXZ, int nY, Action<int3> visitor)
        {
            var nMax = math.max(nXZ, nY);
            for (int n = 0; n < nMax; ++n) {
                for (int y = -n; y <= n; ++y) {
                    for (int z = -n; z <= n; ++z) {
                        for (int x = -n; x <= n; ++x) {
                            if (math.abs(x) < n && math.abs(y) < n && math.abs(z) < n) {
                                continue;
                            }
                            if (math.abs(x) > nXZ || math.abs(y) > nY || math.abs(z) > nXZ) {
                                continue;
                            }
                            visitor(new int3(x, y, z));
                        }
                    }
                }
            }
        }

        public static int RemoveAll<K,V>(this Dictionary<K, V> dict, IEnumerable<K> removes)
        {
            int numRemoved = 0;
            foreach (var k in removes) {
                if (dict.Remove(k))
                    numRemoved++;
            }
            return numRemoved;
        }

        public static int RemoveAll<K>(this HashSet<K> set, IEnumerable<K> removes)
        {
            int numRemoved = 0;
            foreach (var k in removes) {
                if (set.Remove(k))
                    numRemoved++;
            }
            return numRemoved;
        }

        public static string StrDuration(int sec)
        {
            return $"{sec/60/60:00}:{sec/60:00}:{sec % 60:00}";
        }

        public static string StrDayTime(float daytime, bool hr12 = true)
        {
            // 0=6am, 0.25=12am, 0.5=6pm
            float hr = (daytime * 24 + 6) % 24;
            float mn = math.frac(hr) * 60;
            float s = math.frac(mn) * 60;
            
            return $"{math.floor(hr12 ? hr % 13 : hr):00}:{math.floor(mn):00}:{math.floor(s):00}{(hr12? (hr < 13 ? " AM" : " PM") :"")}";
        }
        
        
        
        

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var c = obj.GetComponent<T>();
            if (!c)
                c = obj.AddComponent<T>();
            return c;
        }
        
        public static void LockCursor(bool lockCursor)
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }


        public static bool TryRemoveLast<T>(this List<T> ls)
        {
            if (ls.Count == 0)
                return false;
            ls.RemoveAt(ls.Count-1);
            return true;
        }
        public static int RemoveIf<T>(this List<T> ls, Func<T, bool> predict)
        {
            int numRemoved = 0;
            for (int i = 0; i < ls.Count;) {
                if (predict(ls[i])) {
                    ls.RemoveAt(i);
                    ++numRemoved;
                    continue;
                }
                ++i;
            }
            return numRemoved;
        }

        public static T LastOr<T>(this List<T> ls, T orDefault)
        {
            if (ls.Count == 0)
                return orDefault;
            return ls[ls.Count - 1];
        }

        public static bool IsEmpty<T>(this List<T> ls) => ls.Count == 0;
    }
}