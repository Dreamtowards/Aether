using Unity.Burst;
using Unity.Jobs;
using Unity.Profiling;

namespace Aether
{
    [BurstCompile]
    public struct JobChunkGen : IJob
    {
        static readonly ProfilerMarker s_Pm = new("Aether.JobChunkGen");
        
        
        public NoiseGen _noise;
        public void Execute()
        {
            s_Pm.Begin();
            var s = _noise.Sample(1, 2);
            
            // _chunkGenerator.GenerateChunk(_chunk);
            
            // _chunkGenerator.m_Noise.Sample()

            UnityEngine.Debug.Log($"JobHasDone {s}");
            
            s_Pm.End();
        }
    }
}