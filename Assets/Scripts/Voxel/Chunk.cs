
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class Chunk : MonoBehaviour
    {
        // Should chunk size change dynamically?
        const int LEN = 16;
        const int LEN3 = LEN * LEN * LEN;

        private Vox[] m_Voxels = new Vox[Chunk.LEN3];

        private int3 m_ChunkPos;

        private bool IsPopulated = false;

        private WeakReference<Chunk>[] m_NeighborChunks;

        void Start()
        {

        }
        void Update()
        {

        }

        public ref Vox AtVoxel(int3 localpos)
        {
            return ref m_Voxels[0];
        }

        public static int LocalIdx(int3 localpos)
        {
            return localpos.x << 8 | localpos.y << 4 | localpos.z;
        }
        public static int3 LocalIdxPos(int idx)
        {
            return new((idx >> 8) & 15, (idx >> 4) & 15, idx & 15);
        }

        public static int3 ChunkPos(int3 p)
        {
            return new(Maths.Floor16(p.x), Maths.Floor16(p.y), Maths.Floor16(p.z));
        }
        public static int3 LocalPos(int3 p)
        {
            return new(Maths.Mod16(p.x), Maths.Mod16(p.y), Maths.Mod16(p.z));
        }

        public static bool IsChunkPos(int3 p)
        {
            return math.all(p % 16 == int3.zero);
        }
        public static bool IsLocalPos(int3 p)
        {
            return p.x >= 0 && p.x < 16 &&
                   p.y >= 0 && p.y < 16 &&
                   p.z >= 0 && p.z < 16;
        }
    }
}