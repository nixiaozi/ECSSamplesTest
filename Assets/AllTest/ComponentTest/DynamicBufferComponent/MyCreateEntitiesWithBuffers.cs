using Unity.Entities;
using Unity.Jobs;

[AlwaysUpdateSystem]
[UpdateBefore(typeof(MyTestSystemStateComponentSystem))]
public class MyCreateEntitiesWithBuffers : SystemBase
{
    // A command buffer system executes command buffers in its own OnUpdate
    public EntityCommandBufferSystem CommandBufferSystem;

    protected override void OnCreate()
    {
        // Get the command buffer system
        CommandBufferSystem
            = World.DefaultGameObjectInjectionWorld.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

        // ShouldRunSystem();  // 这个用来查看System有没有达到运行的条件（OnUpdate）
    }

    


    protected override void OnUpdate()
    {
        // The command buffer to record commands,
        // which are executed by the command buffer system later in the frame
        EntityCommandBuffer.Concurrent commandBuffer
            = CommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        //The DataToSpawn component tells us how many entities with buffers to create
        Entities.ForEach((Entity spawnEntity, int entityInQueryIndex, in DataToSpawn data) =>
        {
            for (int e = 0; e < data.EntityCount; e++)
            {
                //Create a new entity for the command buffer
                Entity newEntity = commandBuffer.CreateEntity(entityInQueryIndex);

                //Create the dynamic buffer and add it to the new entity
                DynamicBuffer<MyBufferElement> buffer =
                    commandBuffer.AddBuffer<MyBufferElement>(entityInQueryIndex, newEntity);

                //Reinterpret to plain int buffer
                DynamicBuffer<int> intBuffer = buffer.Reinterpret<int>();

                //Optionally, populate the dynamic buffer
                for (int j = 0; j < data.ElementCount; j++)
                {
                    intBuffer.Add(j);
                }
            }

            //Destroy the DataToSpawn entity since it has done its job
            commandBuffer.DestroyEntity(entityInQueryIndex, spawnEntity);
        }).ScheduleParallel();

        CommandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}



public struct DataToSpawn: ISystemStateComponentData
{
    public int ElementCount;

    public int EntityCount;

}