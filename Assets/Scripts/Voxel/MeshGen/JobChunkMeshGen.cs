using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Aether
{
    [BurstCompile]
    public struct JobChunkMeshGen : IJob
    {
        [ReadOnly]
        public NativeArray<Vox> voxels;
        
        // public Chunk chunk;

        public NativeList<int> ls;
        
        public void Execute()
        {
            ls.Add(3456);
        }
    }
}