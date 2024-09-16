
using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Aether
{
    public class ChunkGenerator : MonoBehaviour
    {
        static readonly ProfilerMarker _ProfilerMarker = new("Aether.ManagedChunkGen");
        
        [ShowInInspector]
        [SerializeField]
        public NoiseGen m_Noise;
        

        
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

                float val = f_terr2d - p.y / 18f + f_3d * 4.5f;

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
                        if (!chunk.GetVoxelOr(new(lx, Chunk.LEN + i, lz)).IsNil()) {
                            distToAir += 1;
                        }
                    }

                    for (int ly = Chunk.LEN-1; ly >= 0; ly--) {
                        int3 lp = new(lx, ly, lz);
                        ref var c = ref chunk.AtVoxel(lp);

                        if (c.IsNil()) {
                            distToAir = 0;
                        } else {
                            distToAir += 1;
                        }

                        var p = chunk.chunkpos + lp;
                        if (c.texId == VoxTex.stone.NumId) {
                            var replace = c.texId;
                            if (distToAir <= 2 && p.y < 10 && m_Noise.Sample(p.x / 32f, p.z / 32f) > 0.1f) {
                                replace = VoxTex.sand.NumId;
                            } else if (distToAir <= 1) {
                                replace = VoxTex.grass.NumId;
                            } else if (distToAir < 3) {
                                replace = VoxTex.dirt.NumId;
                            }
                            c.texId = replace;
                        }
                    }
                }
            }
            
            // Foliage
        }
    }

}