
using Sirenix.OdinInspector;
using System;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aether
{
    public class ChunkGenerator : MonoBehaviour
    {
        static readonly ProfilerMarker s_Pm = new("Aether.ManagedChunkGen");
        
        [ShowInInspector]
        [SerializeField]
        public NoiseGen m_Noise;
        

        
        public void GenerateChunk(Chunk chunk)
        {
            BenchmarkTimer _t = new("Managed GenerateChunk at "+chunk.chunkpos+" in {0}");
            using var _p = s_Pm.Auto();
            m_Noise.Seed = (int)chunk.GetWorld().Seed();
            
            chunk.ForVoxels((int3 localpos, ref Vox vox) =>
            {
                int3 p = chunk.chunkpos + localpos;

                float f_terr2d = m_Noise.Sample(new float2(p.x, p.z) / 130f);
                float f_3d = m_Noise.Sample((float3)p / 90f);

                float val = f_terr2d - p.y / 18f + f_3d * 4.5f;

                if (val > 0)
                    vox.texId = 1;

                vox.density = val;
                vox.shapeId = 1;

            });
            _t.Stop();
        }
    }

}