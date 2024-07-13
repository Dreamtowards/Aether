
using System;
using UnityEngine;

namespace Aether
{
    public struct Vox
    {
        public UInt16 tex_id;

        public UInt16 shape_id;

        public VoxLight light;

        // SDF value for Isosurface Extraction. 0=surface, +positive=solid, -negative=void
        public byte density;
    }

    public struct VoxLight
    {
        private UInt16 m_RGBS;
    }

}