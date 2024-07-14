using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Assertions;

namespace Aether
{
    public class ChunkSystem : MonoBehaviour
    {
        public GameObject m_PrefabChunk;
        
        [ShowInInspector]
        public Dictionary<int3, Chunk> m_Chunks = new();

        public World m_PtrWorld;

        public ChunkGenerator m_ChunkGenerator;

        public HashSet<int3> m_ChunksMeshDirty = new();

        public EntityPlayer m_LoaderPlayer;
        public bool m_NeedChunksLoadance = true;

        void Start()
        {
            var job = new JobChunkGen{};
            JobHandle jobHandle = job.Schedule();
        }

        [Button]
        void Update()
        {
            UpdateChunksLoadance();

            UpdateChunksDirtyMesh();
        }

        private void UpdateChunksLoadance()
        {
            ProvideChunks(m_LoaderPlayer.transform.position, m_LoaderPlayer.m_ChunksLoadDistance);

            foreach (var chunkpos in new List<int3>(m_Chunks.Keys))
            {
                if (!m_LoaderPlayer.InLoadDistance(chunkpos))
                    UnloadChunk(chunkpos);
            }
        }

        private void UpdateChunksDirtyMesh()
        {
            foreach (var chunkpos in m_ChunksMeshDirty)
            {
                if (GetChunk(chunkpos, out var chunk))
                {
                    chunk.RegenerateMesh();
                }
            }
            m_ChunksMeshDirty.Clear();
        }









        #region Accessor

        
        

        public World GetWorld() { return m_PtrWorld; }

        public bool GetChunk(int3 chunkpos, out Chunk chunk)
        {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            return m_Chunks.TryGetValue(chunkpos, out chunk);
        }

        public bool GetVoxel(int3 p, out Vox vox)
        {
            if (GetChunk(Chunk.ChunkPos(p), out Chunk chunk))
            {
                vox = chunk.AtVoxel(Chunk.LocalPos(p));
                return true;
            }
            vox = Vox.Nil;
            return false;
        }

        [Button]
        public Chunk ProvideChunk(int3 chunkpos)
        {
            if (GetChunk(chunkpos, out Chunk chunk))
                return chunk;

            GameObject objChunk = Instantiate(m_PrefabChunk, (float3)chunkpos, Quaternion.identity, transform);
            objChunk.name = $"Chunk ({chunkpos.x}, {chunkpos.y}, {chunkpos.z})";
            chunk = objChunk.GetComponent<Chunk>();
            chunk.InitChunk(chunkpos, this);
            
            m_Chunks.Add(chunkpos, chunk);

            // Load Chunk

            // Generate Chunk

            m_ChunkGenerator.GenerateChunk(chunk);
            
            MarkChunkMeshDirty(chunkpos);

            return chunk;
        }

        [Button]
        public void ProvideChunks(float3 _center, int3 range)
        {
            int3 center = Chunk.ChunkPos(_center);
            // for (int dx = -range.x; dx <= range.x; dx++)
            // for (int dy = -range.y; dy <= range.y; dy++)
            // for (int dz = -range.z; dz <= range.z; dz++)
            // {
            //     var chunkpos = center + new int3(dx, dy, dz) * Chunk.LEN;
            //     ProvideChunk(chunkpos);
            // }

            Utility.ForVolume(-range, range + 1, p =>
            {
                int3 chunkpos = center + p * Chunk.LEN;
                ProvideChunk(chunkpos);
            });
        }

        public bool UnloadChunk(int3 chunkpos)
        {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            if (!m_Chunks.Remove(chunkpos, out Chunk chunk))
                return false;

            DestroyImmediate(chunk.gameObject);
            return true;
        }

        [Button]
        public void UnloadAllChunks()
        {
            while (m_Chunks.Count > 0)
            {
                UnloadChunk(m_Chunks.Last().Key);
            }

#if UNITY_EDITOR
            foreach (var chunk in GetComponentsInChildren<Chunk>())
                DestroyImmediate(chunk.gameObject);
#endif
        }

        public void MarkChunkMeshDirty(int3 chunkpos)
        {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            m_ChunksMeshDirty.Add(chunkpos);
        }
        

        #endregion

        #region Debug Gizmos

        public bool m_DebugDrawChunksOutlines = false;

        void OnDrawGizmos()
        {
            if (m_DebugDrawChunksOutlines)
            {
                Gizmos.color = Color.gray;
                foreach (var chunk in m_Chunks.Values)
                {
                    Gizmos.DrawWireCube((float3)chunk.chunkpos + 8, Vector3.one * 16);
                }
            }
        }

        #endregion
    }

}