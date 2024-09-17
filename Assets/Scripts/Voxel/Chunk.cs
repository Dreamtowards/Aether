
using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace Aether
{
    public class Chunk : MonoBehaviour
    {
        // Should chunk size change dynamically?
        public const int LEN = 16;  // LEN_AXIS
        public const int LEN_VOXLES = LEN * LEN * LEN;  // LEN_VOXELS

        // private NativeArray<Vox> m_Voxels = new(Chunk.LEN_VOXLES, Allocator.Persistent);
        public Vox[] m_Voxels = new Vox[LEN_VOXLES];

        public int3 chunkpos { get; private set; }

        public bool IsPopulated = false;

        [ShowInInspector]
        private ChunkSystem m_InChunkSystem;

        [ShowInInspector]
        private WeakReference<Chunk>[] m_NeighborChunks = new WeakReference<Chunk>[NEIGHBORS.Length];

        public void InitChunk(int3 _chunkpos, ChunkSystem chunksystem)
        {
            chunkpos = _chunkpos;
            Assert.IsTrue(transform.position == (Vector3)(float3)chunkpos);

            m_InChunkSystem = chunksystem;
        }

        public void ForLoadedNeighborChunks(Action<Chunk, int> visitor)
        {
            for (int neibIdx = 0; neibIdx < NEIGHBORS.Length; neibIdx++)
            {
                var neibChunkpos = chunkpos + NEIGHBORS[neibIdx] * Chunk.LEN;
                if (m_InChunkSystem.GetChunk(neibChunkpos, out var neibChunk))
                {
                    visitor(neibChunk, neibIdx);
                }
            }
        }
        public void InitNeighborChunks()
        {
            ForLoadedNeighborChunks((neibChunk, neibIdx) =>
            {
                neibChunk.m_NeighborChunks[Chunk.NeighborIdxOpposite(neibIdx)] = new WeakReference<Chunk>(this); 
                m_NeighborChunks[neibIdx] = new WeakReference<Chunk>(neibChunk);

                // Populate.
                if (neibChunk.IsNeighborsAllLoaded() && !neibChunk.IsPopulated)
                {
                    neibChunk.IsPopulated = true;
                    
                    m_InChunkSystem.m_ChunkGenerator.PopulateChunk(neibChunk);
                    
                    neibChunk.ForLoadedNeighborChunks((nnChunk, nnIdx) =>
                    {
                        if (nnChunk.IsNeighborsAllPopulated())
                        {
                            m_InChunkSystem.MarkChunkMeshDirty(nnChunk.chunkpos);
                        }
                    });
                }
            });
            
            // m_InChunkSystem.MarkChunkMeshDirty(chunkpos);
        }
        public void UnlinkNeighborChunks()
        {
            ForLoadedNeighborChunks((neibChunk, neibIdx) =>
            {
                neibChunk.m_NeighborChunks[Chunk.NeighborIdxOpposite(neibIdx)] = null;
            });
        }

        public ChunkSystem GetChunkSystem() { return m_InChunkSystem; }
        public World GetWorld() { return GetChunkSystem().GetWorld(); }


        void Start()
        {

        }

        void Update()
        {

        }


        public void UploadMesh(Mesh mesh)
        {
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        // Directly Access.
        public ref Vox AtVoxel(int3 localpos) {
            return ref m_Voxels[LocalIdx(localpos)];
        }
        // public ref Vox GetVoxelRef(int3 relpos) {
        //     if (IsLocalPos(relpos)) {
        //         return ref AtVoxel(relpos);
        //     } 
        //     if (GetNeighborChunk(relpos, out var chunk)) {
        //         return ref chunk.AtVoxel(LocalPos(relpos));
        //     }
        //     throw new Exception();
        // }
        public bool GetVoxel(int3 relpos, out Vox vox) {
            if (IsLocalPos(relpos)) {
                vox = AtVoxel(relpos);
                return true;
            }
            if (GetNeighborChunk(relpos, out var chunk)) {
                vox = chunk.AtVoxel(LocalPos(relpos));
                return true;
            }
            vox = Vox.Nil;
            return false;
        }
        public Vox GetVoxelOr(int3 relpos, Vox def = new()) {
            if (GetVoxel(relpos, out Vox vox))
                return vox;
            return def;
        }
        
        public delegate void ModifyVoxelAction(ref Vox vox);
        public bool SetVoxel(int3 relpos, ModifyVoxelAction func) {
            if (IsLocalPos(relpos)) {
                func(ref AtVoxel(relpos));
                return true;
            }
            if (GetNeighborChunk(relpos, out var chunk)) {
                func(ref chunk.AtVoxel(LocalPos(relpos)));
                return true;
            }
            return false;
        }

        public bool SetVoxel(int3 relpos, Vox vox) {
            return SetVoxel(relpos, (ref Vox v) => v = vox);
        }

        public bool GetNeighborChunk(int idx, out Chunk chunk) {
            chunk = null;
            return m_NeighborChunks[idx]?.TryGetTarget(out chunk) == true;
        }
        public bool GetNeighborChunk(int3 relpos, out Chunk chunk) {
            chunk = null;
            return TryNeighborIdx(relpos, out int idx) && GetNeighborChunk(idx, out chunk);
        }

        public static int LocalIdx(int3 localpos) {
            Assert.IsTrue(IsLocalPos(localpos));
            return localpos.x << 8 | localpos.y << 4 | localpos.z;
        }
        public static int3 LocalIdxPos(int idx) {
            Assert.IsTrue(idx >= 0 && idx < LEN_VOXLES);
            return new((idx >> 8) & 15, (idx >> 4) & 15, idx & 15);
        }

        public static int3 ChunkPos(float3 p) { return ChunkPos(Maths.Floor(p)); }
        public static int3 ChunkPos(int3 p) {
            return new(Maths.Floor16(p.x), Maths.Floor16(p.y), Maths.Floor16(p.z));
        }
        public static int3 LocalPos(int3 p) {
            return new(Maths.Mod16(p.x), Maths.Mod16(p.y), Maths.Mod16(p.z));
        }

        public static bool IsChunkPos(int3 p) {
            return math.all(p % 16 == int3.zero);
        }
        public static bool IsLocalPos(int3 p) {
            return p.x >= 0 && p.x < 16 &&
                   p.y >= 0 && p.y < 16 &&
                   p.z >= 0 && p.z < 16;
        }



        public delegate void ForVoxelsAction(int3 localpos, ref Vox vox);
        public void ForVoxels(ForVoxelsAction visitor) {
            for (int i = 0; i < Chunk.LEN_VOXLES; i++) {
                int3 localpos = LocalIdxPos(i);
                visitor(localpos, ref m_Voxels[i]);
            }
        }


        public static readonly int3[] NEIGHBORS =
        {
            // 6 Faces
            new(-1, 0, 0),
            new(1, 0, 0),
            new(0, -1, 0),
            new(0, 1, 0),
            new(0, 0, -1),
            new(0, 0, 1),
            // 12 Edges
            new(0, -1, -1), // X
            new(0, 1, 1),
            new(0, 1, -1),
            new(0, -1, 1),
            new(-1, 0, -1), // Y
            new(1, 0, 1),
            new(1, 0, -1),
            new(-1, 0, 1),
            new(-1, -1, 0), // Z
            new(1, 1, 0),
            new(-1, 1, 0),
            new(1, -1, 0),
            // 8 Vertices
            new(-1, -1, -1),
            new(1, 1, 1),
            new(1, -1, -1),
            new(-1, 1, 1),
            new(-1, -1, 1),
            new(1, 1, -1),
            new(1, -1, 1),
            new(-1, 1, -1),
        };

        // relpos (relative localpos) is like localpos except it allows outbound of Chunk.LEN but still in range MinMax +- Chunk.LEN.
        public static int NeighborIdx(int3 relpos)
        {
            for (int i = 0; i < NEIGHBORS.Length; i++)
            {
                if (IsLocalPos(relpos - NEIGHBORS[i] * Chunk.LEN))
                    return i;
            }
            return -1;
        }
        public static bool TryNeighborIdx(int3 relpos, out int idx)
        {
            idx = NeighborIdx(relpos);
            return idx != -1;
        }

        // assert!(Self::NEIGHBOR_DIR[idx] + Self::NEIGHBOR_DIR[opposite_idx] == IVec3::ZERO, "idx = {}, opposite = {}", idx, opposite_idx);
        public static int NeighborIdxOpposite(int idx)
        {
            // idx MOD 2 + (idx+1) MOD 2
            return idx / 2 * 2 + (idx + 1) % 2;
        }

        public bool IsNeighborsAllLoaded() {
            foreach (var neib in m_NeighborChunks) {
                if (neib == null)
                    return false;
            }
            return true;
        }

        public bool IsNeighborsAllPopulated() {
            foreach (var neib in m_NeighborChunks) {
                if (neib == null || !neib.TryGetTarget(out Chunk chunk) || !chunk.IsPopulated)
                    return false;
            }
            return true;
        }
    }
}