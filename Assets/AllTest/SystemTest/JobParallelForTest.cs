
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


            // if(math.)

        }
    }

    private EntityQuery m_Query;

    protected override void OnCreate()
    {
        m_Query = GetEntityQuery(typeof(B1));
    }

    protected override void OnUpdate()
    {

        var chunks = m_Query.CreateArchetypeChunkArray(Allocator.TempJob);


        // throw new System.NotImplementedException();
        var job = new ParallelForTestJob
        {
            Chunks = chunks,//EntityManager.GetAllChunks(),
            DeltaTime = 0.3f,
            LastSystemVersion = this.LastSystemVersion,
            TypeB1 = GetArchetypeChunkComponentType<B1>(false),
            TypeB2 = GetArchetypeChunkComponentType<B2>(true)
        };

        this.Dependency = job.Schedule(chunks.Length, 32, this.Dependency);

    }
}



public struct B1 : IComponentData { }

public struct B2 : IComponentData { }