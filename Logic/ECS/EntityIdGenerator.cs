namespace GodotMonoGeneral.Logic.ECS;

using System.Collections.Generic;

public class EntityIdGenerator
{
    private const int ChunkSize = 64; // 使用ulong，每个chunk 64个ID
    private readonly List<ulong> chunks = new List<ulong>();
    private int nextAvailableChunk = 0;
    
    public EntityIdGenerator()
    {
        int initialChunks = (1024 + ChunkSize - 1) / ChunkSize;
        for (int i = 0; i < initialChunks; i++)
        {
            chunks.Add(0); // 初始化为全0，表示所有ID可用
        }
    }
    
    public int Next()
    {
        // 首先在当前chunk中查找
        for (int chunkIndex = nextAvailableChunk; chunkIndex < chunks.Count; chunkIndex++)
        {
            ulong chunk = chunks[chunkIndex];
            if (chunk != ulong.MaxValue) // 如果chunk不是全满
            {
                int bitIndex = FindFirstZeroBit(chunk);
                if (bitIndex >= 0)
                {
                    chunks[chunkIndex] = chunk | (1UL << bitIndex);
                    nextAvailableChunk = chunkIndex;
                    return chunkIndex * ChunkSize + bitIndex;
                }
            }
        }
        
        // 没有找到可用ID，添加新的chunk
        chunks.Add(1UL); // 第一个bit被使用
        nextAvailableChunk = chunks.Count - 1;
        return (chunks.Count - 1) * ChunkSize;
    }
    
    public void ReturnEntityId(int id)
    {
        int chunkIndex = id / ChunkSize;
        int bitIndex = id % ChunkSize;
        
        if (chunkIndex < chunks.Count)
        {
            chunks[chunkIndex] &= ~(1UL << bitIndex);
            if (chunkIndex < nextAvailableChunk)
            {
                nextAvailableChunk = chunkIndex;
            }
        }
    }
    
    private int FindFirstZeroBit(ulong value)
    {
        for (int i = 0; i < ChunkSize; i++)
        {
            if ((value & (1UL << i)) == 0)
            {
                return i;
            }
        }
        return -1;
    }
    
    public int GetMemoryUsage() => chunks.Count * sizeof(ulong);
}