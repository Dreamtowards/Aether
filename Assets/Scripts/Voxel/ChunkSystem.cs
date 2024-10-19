using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Aether
{
    public class ChunkSystem : MonoBehaviour
    {
        public GameObject m_PrefabChunk;
        
        [ShowInInspector]
        public Dictionary<int3, Chunk> m_Chunks = new();

        public World m_InWorld;

        public ChunkLoadMarker m_ChunkLoadMarker;
        
        // commonly only need when Visitor/Player moved across chunk or Appear/Disappear.
        // public bool m_NeedUpdateChunksLoadance = true;

        public static ChunkSystem instance;

        void Start()
        {
            Assert.IsNull(instance);
            instance = this;
        }

        [Button]
        void Update()
        {
            UpdateChunksLoadAndUnload();

            UpdateChunksDirtyMesh();
        }
        
#if UNITY_EDITOR
        [Button]
        void EditorLoadChunks()
        {
            do
            {
                Update();
            } while (m_ChunksLoading.Count > 0 || m_ChunksMeshing.Count > 0);
        }
#endif

        #region Chunks Loading/Unload

        [BoxGroup("ChunkGen")]
        public ChunkGenerator m_ChunkGenerator;
        
        [BoxGroup("ChunkGen")]
        [ShowInInspector]
        public Dictionary<int3, Task<Chunk>> m_ChunksLoading = new();
        
        [BoxGroup("ChunkGen")]
        public int m_ChunksLoadingMaxConcurrency = 10;

        private void UpdateChunksLoadAndUnload()
        {
            // Fetch Completed LoadingChunk Tasks

            HashSet<int3> chunksLoaded = new();
            foreach (var (chunkpos, task) in m_ChunksLoading)
            {
                if (!task.IsCompleted)
                    continue;
                var chunk = task.Result;
                
                chunk.InitNeighborChunks();
                // MarkChunkMeshDirty(chunkpos);
                
                m_Chunks.Add(chunkpos, chunk);
                chunksLoaded.Add(chunkpos);
            }
            m_ChunksLoading.RemoveAll(chunksLoaded);
            
            
            // Loading Chunks :: Dispatch Task
            
            int3 center = Chunk.ChunkPos(m_ChunkLoadMarker.transform.position);
            int3 range = m_ChunkLoadMarker.m_ChunksLoadDistance;

            Utility.ForVolumeSpread(range.x, range.y, p =>
            {
                int3 chunkpos = center + p * Chunk.LEN;

                if (m_ChunksLoading.Count >= m_ChunksLoadingMaxConcurrency)
                    return;
                if (m_ChunksLoading.ContainsKey(chunkpos))
                    return;
                if (HasChunk(chunkpos))
                    return;

                // Load Chunk
                // Generate Chunk

                GameObject objChunk = Instantiate(m_PrefabChunk, (float3)chunkpos, Quaternion.identity, transform);
                objChunk.name = $"Chunk ({chunkpos.x}, {chunkpos.y}, {chunkpos.z})";
                Chunk chunk = objChunk.GetComponent<Chunk>();
                chunk.InitChunk(chunkpos, this);
                
                var task = new Task<Chunk>(() =>
                {
                    m_ChunkGenerator.GenerateChunk(chunk);

                    return chunk;
                });
                task.Start();
                m_ChunksLoading.Add(chunkpos, task);
            });
            
            // Unload chunks
            foreach (var chunkpos in new List<int3>(m_Chunks.Keys))
            {
                if (!m_ChunkLoadMarker.InLoadDistance(chunkpos))
                    UnloadChunk(chunkpos);
            }
        }

        #endregion



        #region Chunks Meshing

        [BoxGroup("MeshGen")]
        [ShowInInspector]
        public HashSet<int3> m_ChunksMeshDirty = new();
        
        [BoxGroup("MeshGen")]
        [ShowInInspector]
        public Dictionary<int3, Task<VertexBuffer>> m_ChunksMeshing = new();
        
        [BoxGroup("MeshGen")]
        public int m_ChunksMeshingMaxConcurrency = 10;

        private static ObjectPool<VertexBuffer> s_VertexBufferPool = new(() => new VertexBuffer());

        [BoxGroup("MeshGen")]
        [ShowInInspector]
        public static int MeshgenVbufPoolNumAll => s_VertexBufferPool.CountAll;
        [BoxGroup("MeshGen")]
        [ShowInInspector]
        public static int MeshgenVbufPoolNumActive => s_VertexBufferPool.CountActive;
        
        private void UpdateChunksDirtyMesh()
        {
            HashSet<int3> chunksMeshed = new();
            foreach (var (chunkpos, task) in m_ChunksMeshing)
            {
                if (!task.IsCompleted)
                    continue;
                var vbuf = task.Result;

                if (GetChunk(chunkpos, out var chunk))
                {
                    vbuf.ComputeNormalsSmooth();
                    chunk.UploadMesh(vbuf.ToMesh());
                }
                s_VertexBufferPool.Release(vbuf);
                chunksMeshed.Add(chunkpos);
            }
            m_ChunksMeshing.RemoveAll(chunksMeshed);
            
            
            HashSet<int3> chunksTasked = new();
            foreach (var chunkpos in m_ChunksMeshDirty)
            {
                if (m_ChunksMeshing.Count >= m_ChunksMeshingMaxConcurrency)
                    continue;
                if (m_ChunksMeshing.ContainsKey(chunkpos))
                    continue;
                if (!GetChunk(chunkpos, out var chunk))
                    continue;

                VertexBuffer vbuf = s_VertexBufferPool.Get();

                var task = new Task<VertexBuffer>(() =>
                {
                    vbuf.Clear();
                    
                    ChunkMeshGenerator.GenerateMesh(vbuf, chunk);
                    
                    return vbuf;
                });
                task.Start();
                
                m_ChunksMeshing.Add(chunkpos, task);
                chunksTasked.Add(chunkpos);
            }
            m_ChunksMeshDirty.RemoveAll(chunksTasked);
        }

        #endregion









        #region Voxel Accessor

        
        public World GetWorld() { return m_InWorld; }

        public bool GetChunk(int3 chunkpos, out Chunk chunk) {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            return m_Chunks.TryGetValue(chunkpos, out chunk);
        }

        public bool HasChunk(int3 chunkpos) {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            return m_Chunks.ContainsKey(chunkpos);
        }

        public int NumChunks => m_Chunks.Count;

        public bool GetVoxel(int3 p, out Vox vox) {
            if (GetChunk(Chunk.ChunkPos(p), out Chunk chunk)) {
                vox = chunk.AtVoxel(Chunk.LocalPos(p));
                return true;
            }
            vox = Vox.Nil;
            return false;
        }

        public bool UnloadChunk(int3 chunkpos)
        {
            Assert.IsTrue(Chunk.IsChunkPos(chunkpos));
            if (!m_Chunks.Remove(chunkpos, out Chunk chunk))
                return false;
            chunk.UnlinkNeighborChunks();
            
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
        
        
        
        

        public void ModifySphere(float3 point, float radius, float intensity = 1, int texId = -1)
        {
            int3 lastChunkPos;
            if (!GetChunk(lastChunkPos=Chunk.ChunkPos(point), out var chunk))
                return;
            var lpMid = Chunk.LocalPos(point);

            MarkChunkMeshDirty(lastChunkPos);
            Utility.ForVolumeMid(0, (int)math.ceil(radius), p =>
            {
                chunk.SetVoxel(lpMid + p, (ref Vox vox) =>
                {
                    float dist = math.length(p);
                    float f = math.max(0, radius - dist);
                    vox.density += f*intensity;
                    if (texId > 0 && vox.density > 0 && f > 0) {
                        vox.texId = (ushort)texId;
                    }
                });
                var cp = Chunk.ChunkPos((int3)point + p);
                if (math.any(lastChunkPos != cp)) {
                    MarkChunkMeshDirty(lastChunkPos=cp);
                }
            });
        }
    }

}