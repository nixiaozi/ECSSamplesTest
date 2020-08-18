
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class JobParallelForTest : SystemBase
{
    struct ParallelForTestJob : IJobParallelFor
    {
        [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> Chunks;
        public ArchetypeChunkComponentType<B1> TypeB1;
        [ReadOnly] public ArchetypeChunkComponentType<B2> TypeB2;

        public float DeltaTime;
        public uint LastSystemVersion;


        public void Execute(int chunkIndex) // 这里的参数当前迭代的Chunk索引
        {
            var chunk = Chunks[chunkIndex];
            var chunkB1 = chunk.GetNativeArray(TypeB1);
            var chunkB2 = chunk.GetNativeArray(TypeB2);
            var instanceCount = chunk.Count;


            if(math.)

        }
    }



    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}



public struct B1 : IComponentData { }

public struct B2 : IComponentData { }