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
        // ���ȴ���һ��ԭ�ͣ�Ȼ��ͨ������ԭ�ʹ���ʵ��
        var archetype = EntityManager.CreateArchetype(typeof(EntityAge_ForEach) );  // ���������������ɵ��ǲ��������Archetype

        EntityManager.CreateEntity(archetype,5, Allocator.Temp);

        // Create an entity with components that use an array of ComponentType objects.
        EntityManager.CreateEntity(typeof(EntityAge_ForEach), typeof(EntityCount_ForEach));

        // Create an entity with components that use an EntityArchetype.
        var instance = EntityManager.CreateEntity(archetype);

        // Copy an existing entity, including its current data, with Instantiate
        EntityManager.Instantiate(instance);

        // ������һ��û�з���ֵ�����÷�����������Entity
        // var entityArray = new NativeArray<Entity>(11, Allocator.TempJob); // ���û������飬�������������շ�ʽ
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
            .ForEach((ref Rotation rotation, in RotationSpeed_ForEach rotationSpeed) =>  // RotationSpeed_ForEach  ��������������Rotation ����������ı���
            {
                rotation.Value = math.mul(
                    math.normalize(rotation.Value), 
                    quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * deltaTime));  // ��ȡ��ǰ�Ƕȣ������޸ĵ�ǰ�Ƕ�
            })
            .ScheduleParallel();
    }


    protected override void OnDestroy()   //  ԭ����OnDestroyManager �Ѿ��Ƴ�
    {
        entityArray.Dispose();
    }

}
