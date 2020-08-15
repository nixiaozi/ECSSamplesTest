using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// This system updates all entities in the scene with both a RotationSpeed_ForEach and Rotation component.

// ReSharper disable once InconsistentNaming
[UpdateBefore(typeof(TestSystem_ForEach))]
public class RotationSpeedSystem_ForEach : SystemBase
{
    NativeArray<Entity> entityArray = new NativeArray<Entity>(11, Allocator.TempJob);

    protected override void OnCreate()
    {
        // 首先创建一个原型，然后通过比照原型创建实体
        var archetype = EntityManager.CreateArchetype(typeof(EntityAge_ForEach) );  // 不带参数代表生成的是不带组件的Archetype

        EntityManager.CreateEntity(archetype,5, Allocator.Temp);

        // Create an entity with components that use an array of ComponentType objects.
        EntityManager.CreateEntity(typeof(EntityAge_ForEach), typeof(EntityCount_ForEach));

        // Create an entity with components that use an EntityArchetype.
        var instance = EntityManager.CreateEntity(archetype);

        // Copy an existing entity, including its current data, with Instantiate
        EntityManager.Instantiate(instance);

        // 这里有一个没有返回值的引用方法用来生成Entity
        // var entityArray = new NativeArray<Entity>(11, Allocator.TempJob); // 设置缓存数组，和数组垃圾回收方式
        EntityManager.CreateEntity(archetype, entityArray);


        // Create an entity with no components and then add components to it. (You can add components immediately or when additional components are needed.)
        var entity = EntityManager.CreateEntity();
        EntityManager.AddComponent(entity, typeof(EntityCount_ForEach));

    }


    // OnUpdate runs on the main thread.
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        

        // Schedule job to rotate around up vector
        Entities
            .WithName("RotationSpeedSystem_ForEach")
            .ForEach((ref Rotation rotation, in RotationSpeed_ForEach rotationSpeed) =>  // RotationSpeed_ForEach  这个是输入参数，Rotation 是输出参数改变量
            {
                rotation.Value = math.mul(
                    math.normalize(rotation.Value), 
                    quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * deltaTime));  // 获取当前角度，并且修改当前角度
            })
            .ScheduleParallel();
    }


    protected override void OnDestroy()   //  原来的OnDestroyManager 已经移除
    {
        entityArray.Dispose();
    }

}
