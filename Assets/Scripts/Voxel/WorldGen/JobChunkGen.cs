using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;

namespace Aether
{
    [BurstCompile]
    public struct JobChunkGen : IJob
    {
        static readonly ProfilerMarker _ProfilerMarker = new("Aether.JobChunkGen");

        
        public int3 chunkpos;
        public NoiseGenStruct _noise;

        public NativeArray<Vox> voxels;
        
        
        public void Execute()
        {
            using var _s = _ProfilerMarker.Auto();

            for (int i = 0; i < Chunk.LEN_VOXLES; i++)
            {
                int3 localpos = Chunk.LocalIdxPos(i);
                int3 p = chunkpos + localpos;
                
                float f_terr2d = _noise.Sample(new float2(p.x, p.z) / 130f);
                float f_3d = _noise.Sample((float3)p / 90f);

                float val = f_terr2d - p.y / 18f + f_3d * 4.5f;

                Vox vox = new();
                if (val > 0)
                    vox.texId = 1;

                vox.density = val;
                vox.shapeId = 1;
                voxels[i] = vox;
            }

            UnityEngine.Debug.Log($"Job ChunkGen at {chunkpos} Has Completed");
        }
    }
}