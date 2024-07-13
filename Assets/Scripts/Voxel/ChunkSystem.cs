using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class ChunkSystem : MonoBehaviour
    {
        [ShowInInspector]
        public Dictionary<int3, Chunk> m_Chunks = new();

        public World m_PtrWorld;

        public ChunkGenerator m_ChunkGenerator;

        public GameObject m_PrefabChunk;

        void Start()
        {
            m_ChunkGenerator = GetComponent<ChunkGenerator>();
        }

        void Update()
        {

        }

        public World GetWorld() { return m_PtrWorld; }

        // return: maybe null if the chunk has not been loaded
        public Chunk GetChunk(int3 chunkpos)
        {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            if (m_Chunks.TryGetValue(chunkpos, out Chunk chunk))
                return chunk;
            return null;
        }

        [Button]
        public Chunk ProvideChunk(int3 chunkpos)
        {
            Chunk chunk = GetChunk(chunkpos);
            if (chunk != null)
                return chunk;

            GameObject objChunk = Instantiate(m_PrefabChunk, (float3)chunkpos, Quaternion.identity, transform);
            objChunk.name = $"Chunk ({chunkpos.x}, {chunkpos.y}, {chunkpos.z})";
            chunk = objChunk.GetComponent<Chunk>();
            chunk.InitChunk(chunkpos, this);
            
            m_Chunks.Add(chunkpos, chunk);

            // Load Chunk

            // Generate Chunk

            m_ChunkGenerator.GenerateChunk(chunk);

            return chunk;
        }

        [Button]
        public void ProvideChunks(int3 center, int3 range)
        {
            Assert.IsTrue(Chunk.IsChunkPos(center));
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

        [Button]
        public void UnloadAllChunks()
        {
            foreach (var (key, chunk) in m_Chunks)
            {
                DestroyImmediate(chunk.gameObject);
            }
            m_Chunks.Clear();

#if UNITY_EDITOR
            foreach (var chunk in GetComponentsInChildren<Chunk>())
            {
                DestroyImmediate(chunk.gameObject);
            }
#endif
        }

        public bool m_DebugDrawChunksOutlines = false;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            foreach (var chunk in m_Chunks.Values)
            {
                Gizmos.DrawWireCube((float3)chunk.chunkpos + 8, Vector3.one * 16);
            }
        }
    }

}