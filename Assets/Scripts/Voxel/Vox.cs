
using System;

namespace Aether
{
    public struct Vox
    {
        public UInt16 texId;

        public UInt16 shapeId;

        public VoxLight light;

        // SDF value for Isosurface Extraction. 0=surface, +positive=solid, -negative=void
        public float density;

        public static readonly Vox Nil = new();


        public bool IsNil()
        {
            return texId == 0;
        }
        public bool IsOpaque()
        {
            return !IsNil();
        }
    }

    public struct VoxLight
    {
        private UInt16 m_RGBS;
    }

}