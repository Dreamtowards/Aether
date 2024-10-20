
using System;
using Unity.Mathematics;

namespace Aether
{
    // global 
    using VoxTexId = System.UInt16;
    
    public struct Vox
    {
        public VoxTexId texId;

        public UInt16 shapeId;

        public VoxLight light;

        // SDF value for Isosurface Extraction. 0=surface, +positive=solid, -negative=void
        public float density;

        public float3 CachedFp;
        public float3 CachedNorm;

        public static readonly Vox Nil = new();


        public bool IsTexNil() => texId == 0;

        public bool IsIsoNil() => density <= 0;
        

        // public bool IsNil()
        // {
        //     return texId == 0;
        // }
        // public bool IsOpaque()
        // {
        //     return !IsNil();
        // }
        // public bool IsDensitySolid() => density > 0;
    }

    public struct VoxLight
    {
        private UInt16 m_RGBS;
    }

}