
using Sirenix.OdinInspector;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class ChunkGenerator : MonoBehaviour
    {
        [ShowInInspector]
        [SerializeField]
        public Noise m_Noise = new();

        public void GenerateChunk(Chunk chunk)
        {
            m_Noise.Seed = (int)chunk.GetWorld().Seed();

            chunk.ForVoxels((int3 localpos, ref Vox vox) =>
            {
                // if (localpos.y < 10)
                // {
                //     vox.texId = 1;
                //     vox.density = 1;
                // }
                // return;
                
                int3 p = chunk.chunkpos + localpos;

                float f_terr2d = m_Noise.Sample(new float2(p.x, p.z) / 130f);
                float f_3d = m_Noise.Sample((float3)p / 90f);

                float val = f_terr2d - p.y / 18f + f_3d * 4.5f;

                if (val > 0)
                    vox.texId = 1;

                vox.density = val;
                vox.shapeId = 1;

            });
        }
    }

}