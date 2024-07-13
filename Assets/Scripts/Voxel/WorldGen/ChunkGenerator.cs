
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class ChunkGenerator : MonoBehaviour
    {
        public FastNoiseLite m_Noise = new();


        public float m_SamplingFactor = 0.1f;

        public void GenerateChunk(Chunk chunk)
        {
            var seed = chunk.GetWorld().Seed();

            chunk.ForVoxels((int3 localpos, ref Vox vox) =>
            {
                int3 p = chunk.chunkpos + localpos;

                m_Noise.GetNoise(p.x * m_SamplingFactor, p.y * m_SamplingFactor, p.z * m_SamplingFactor);

            });
        }
    }
}