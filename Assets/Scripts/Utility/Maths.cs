using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public static class Maths
    {
        public static int Floor16(int i)
        {
            return i & (~15);
        }

        public static int Mod16(int i)
        {
            return i & 15;
        }

        // note: MOD != REM (%), e.g. -3 MOD 16 -> 13 while -3 REM 16 = 3
        public static float Mod(float v, float n)
        {
            float f = v % n;
            return f < 0 ? f + n : f;
            // or return ((v % n) + n) % n;
            // or dummy return floor(v / n) * n;
        }

        public static float Floor(float v, float n)
        {
            return Mathf.Floor(v / n) * n;
            //float f = (int)(v / n) * n;
            //return v < 0 ? f - n : f;
        }

        public static int3 Floor(float3 v)
        {
            return (int3)math.floor(v);
        }



        // CollectionsUtility

        public static int[] Sequence(int n, int begin = 0)
        {
            int[] seq = new int[n];
            for (int i = 0; i < n; ++i)
            {
                seq[i] = i + begin;
            }
            return seq;
        }

        // VectorUtility

        public static float3 Vec3(float[] arr, int idx)
        {
            return new(arr[idx], arr[idx + 1], arr[idx + 2]);
        }
        public static int3 IVec3(int[] arr, int idx)
        {
            return new(arr[idx], arr[idx + 1], arr[idx + 2]);
        }
        public static float2 Vec2(float[] arr, int idx)
        {
            return new(arr[idx], arr[idx + 1]);
        }
    }
}