
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public struct GeneralPurposeComponentA : IComponentData
{
    public int Lifetime;
}

public struct StateComponentB : ISystemStateComponentData
{
    public int State;
}

public class MyTestSystemStateComponentSystem : SystemBase
{
    private EntityCommandBufferSystem ecbSource;

    protected override void OnCreate()
    {
        ecbSource = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

        // Create some test entities
        // This runs on the main thread, but it is still faster to use a command buffer
        EntityCommandBuffer creationBuffer = new EntityCommandBuffer(Allocator.Temp);  // 使用实体命令缓存
        EntityArchetype archetype = EntityManager.CreateArchetype(typeof(GeneralPurposeComponentA)); // 创建一个原型
        for (int i = 0; i < 10000; i++)
        {
            Entity newEntity = creationBuffer.CreateEntity(archetype);
            creationBuffer.SetComponent<GeneralPurposeComponentA>
            (
                newEntity,
                new GeneralPurposeComponentA() { Lifetime = i }
            );
        }
        //Execute the command buffer
        creationBuffer.Playback(EntityManager);
    }

    protected override void OnUpdate()
    {


        // EntityCommandBuffer.ParallelWriter parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter(); // 一直出现 ParallelWriter AsParallelWriter() 定义不存在问题
        EntityCommandBuffer entityCommandBuffer = ecbSource.CreateCommandBuffer();
        var parallelWriterECB = entityCommandBuffer.ToConcurrent();
        

        // Entities with GeneralPurposeComponentA but not StateComponentB
        Entities
            .WithNone<StateComponentB>()
            .ForEach(
                (Entity entity, int entityInQueryIndex, in GeneralPurposeComponentA gpA) =>
                {
                    // Add an ISystemStateComponentData instance
                    // parallelWriterECB.AddComponent<StateComponentB> //替换修改错误
                    // entityCommandBuffer.AddComponent<StateComponentB>  // 可以使用带jobId的操作
                    parallelWriterECB.AddComponent<StateComponentB>
                        (
                            entityInQueryIndex,
                            entity,
                            new StateComponentB() { State = 1 }
                        );
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

        // Create new command buffer
        // parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter(); // bug 修复
        // entityCommandBuffer = ecbSource.CreateCommandBuffer();
        parallelWriterECB = ecbSource.CreateCommandBuffer().ToConcurrent();

        // Entities with both GeneralPurposeComponentA and StateComponentB
        Entities
            .WithAll<StateComponentB>()
            .ForEach(
                (Entity entity,
                 int entityInQueryIndex,
                 ref GeneralPurposeComponentA gpA) =>
                {
                    // Process entity, in this case by decrementing the Lifetime count
                    gpA.Lifetime--;

                    // If out of time, destroy the entity
                    if (gpA.Lifetime <= 0)
                    {
                        // parallelWriterECB.DestroyEntity(entityInQueryIndex, entity); // bug 修复 可能已经自动定位了Index
                        // entityCommandBuffer.DestroyEntity(entity);
                        parallelWriterECB.DestroyEntity(entityInQueryIndex, entity);
                    }
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

        // Create new command buffer
        // parallelWriterECB = ecbSource.CreateCommandBuffer().AsParallelWriter(); // bug 修复
        // entityCommandBuffer = ecbSource.CreateCommandBuffer();
        parallelWriterECB = ecbSource.CreateCommandBuffer().ToConcurrent();

        // Entities with StateComponentB but not GeneralPurposeComponentA
        Entities
            .WithAll<StateComponentB>()
            .WithNone<GeneralPurposeComponentA>()
            .ForEach(
                (Entity entity, int entityInQueryIndex) =>
                {
                    // This system is responsible for removing any ISystemStateComponentData instances it adds
                    // Otherwise, the entity is never truly destroyed.
                    // parallelWriterECB.RemoveComponent<StateComponentB>(entityInQueryIndex, entity); // bug 修复
                    // entityCommandBuffer.RemoveComponent<StateComponentB>(entity);
                    parallelWriterECB.RemoveComponent<StateComponentB>(entityInQueryIndex, entity);
                })
            .ScheduleParallel();
        ecbSource.AddJobHandleForProducer(this.Dependency);

    }

    protected override void OnDestroy()
    {
        // Implement OnDestroy to cleanup any resources allocated by this system.
        // (This simplified example does not allocate any resources, so there is nothing to clean up.)
    }
}

