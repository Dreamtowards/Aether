
using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Aether
{
    public class ChunkGenerator : MonoBehaviour
    {
        static readonly ProfilerMarker _ProfilerMarker = new("Aether.ManagedChunkGen");
        
        [ShowInInspector]
        [SerializeField]
        public NoiseGen m_Noise;


        public float m_TerrainAvgHeight = 8f;
        public float m_Noise3DFactor = 2.0f;
        public Vector2 m_IslandRadiusHeight = new(40, 10);


        [BoxGroup("Prefabs")]
        public GameObject 
            m_TreeOak,
            m_TreeConifer,
            m_TreePalm,
            m_TreePalmSteep,
            m_Bush,
            m_BushRed,
            m_Fern,
            m_Grass,
            m_Shrub,
            m_Heather;

        
        public void GenerateChunk(Chunk chunk)
        {
            using var _t = new BenchmarkTimer("Managed GenerateChunk at "+chunk.chunkpos+" in {0}");
            using var _p = _ProfilerMarker.Auto();
            // m_Noise.Seed = (int)chunk.GetWorld().Seed;
            
            chunk.ForVoxels((int3 localpos, ref Vox vox) =>
            {
                int3 p = chunk.chunkpos + localpos;

                float f_terr2d = m_Noise.Sample(new float2(p.x, p.z) / 130f);
                float f_3d = m_Noise.Sample((float3)p / 90f);

                float val = f_terr2d - p.y / m_TerrainAvgHeight + f_3d * m_Noise3DFactor;

                // Island the Cone.
                // float xzDistToOrigin = math.length(p.xz);
                // val += (m_IslandRadiusHeight.x - xzDistToOrigin) / m_IslandRadiusHeight.x + (1.0f - p.y / m_IslandRadiusHeight.y);
                //
                // val += f_terr2d - p.y / m_TerrainAvgHeight + f_3d * m_Noise3DFactor;
                
                if (val > 0)
                    vox.texId = VoxTex.stone.NumId;

                vox.density = val;
                vox.shapeId = 1;

            });
        }


        public void PopulateChunk(Chunk chunk)
        {
            using var _t = new BenchmarkTimer("Managed GeneratePopulation at "+chunk.chunkpos+" in {0}");
            
            // Surface Replacement
            for (int lx = 0; lx < Chunk.LEN; lx++) {
                for (int lz = 0; lz < Chunk.LEN; lz++) {
                    
                    // distance to air in top direction.
                    int distToAir = 0;

                    // check top air_dist. for CubicChunk system, otherwise the chunk-top will be surface/grass
                    for (int i = 0; i < 3; ++i) {
                        if (!chunk.GetVoxelOr(new(lx, Chunk.LEN + i, lz)).IsTexNil()) {
                            distToAir += 1;
                        }
                    }

                    for (int ly = Chunk.LEN-1; ly >= 0; ly--) {
                        int3 lp = new(lx, ly, lz);
                        ref var c = ref chunk.AtVoxel(lp);

                        if (c.IsTexNil() || c.IsIsoNil()) {
                            distToAir = 0;
                            continue;
                        }
                        distToAir += 1;
                            
                        var p = chunk.chunkpos + lp;
                        if (c.texId == VoxTex.stone.NumId) {
                            var replace = c.texId;
                            if (distToAir <= 2 && p.y < 2) {// && m_Noise.Sample(p.x / 32f, p.z / 32f) > 0.1f) {
                                replace = VoxTex.sand.NumId;
                            } else if (distToAir <= 1) {
                                replace = VoxTex.grass.NumId;
                            } else if (distToAir < 3) {
                                replace = VoxTex.dirt.NumId;
                            } else if (distToAir > 5) {
                                replace = VoxTex.rock.NumId;
                            }
                            c.texId = replace;
                        }

                        if (distToAir == 1 && ly < Chunk.LEN-1 && p.y > 0)
                        {
                            var fGrass = m_Noise.Sample(p.x / 16f, p.z / 16f);
                            GameObject spawn = null;
                            if (c.texId == VoxTex.grass.NumId && fGrass > 0.3f)
                            {
                                spawn = m_Grass;
                                if (fGrass > 0.4) spawn = m_Bush;
                                if (fGrass > 0.5) spawn = m_Fern;
                                if (fGrass > 0.6) spawn = m_BushRed;
                                if (fGrass > 0.7) spawn = m_Heather;
                                
                                if (hash(p.xz, 3522) < (2 / 256f))
                                    spawn = m_TreeConifer;
                                if (hash(p.xz, 9737) < (2 / 256f))
                                    spawn = m_TreeOak;
                            }
                            else if (c.texId == VoxTex.sand.NumId && fGrass > 0.4f)
                            {
                                spawn = m_Shrub;

                                if (hash(p.xz, 2411) < (6 / 256f))
                                    spawn = m_TreePalm;
                                if (hash(p.xz, 2411) < (2 / 256f))
                                    spawn = m_TreePalmSteep;
                            }

                            if (spawn)
                                PutPrefabAt(chunk, spawn, lp);
                        }
                    }
                }
            }
            
            // Foliage
        }

        private static void PutPrefabAt(Chunk chunk, GameObject prefab, int3 localpos)
        {
            var obj = Instantiate(prefab, chunk.transform);
            obj.transform.localPosition = localpos + new float3(0.5f, 0.5f, 0.5f) + (float3)Random.insideUnitSphere* 0.5f;
            obj.transform.Rotate(Vector3.up, Random.value * 360.0f);
        }

        // RETURN: [0, 1]
        private static float hash(int v) {
            var i = (v << 13) ^ v;
            return ((i * i * 15731 + 789221) * i + 1376312589 & 0xffffffff) / (float)0xffffffff;
        }

        private static float hash(int2 p, int salt) {
            return hash(p.x * salt*salt ^ (p.y + salt));
        }
    }

}