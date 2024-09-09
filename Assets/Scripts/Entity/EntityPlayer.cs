
using Unity.Mathematics;

namespace Aether
{
    public class EntityPlayer : Entity
    {
        public int3 m_ChunksLoadDistance = new(1,1,1);

        public bool InLoadDistance(int3 chunkpos)
        {
            var center = Chunk.ChunkPos(transform.position);
            return math.all(math.abs(center - chunkpos) <= m_ChunksLoadDistance * Chunk.LEN);
        }

    }
}