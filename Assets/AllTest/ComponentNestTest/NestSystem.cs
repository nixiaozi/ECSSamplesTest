using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class NestSystem : SystemBase
{
    protected override void OnCreate()
    {
        // ArgumentException: Nest1 contains a field of Unity.Collections.LowLevel.Unsafe.DisposeSentinel, which is neither primitive nor blittable.
        //// 新建Nest1 实体 
        //var entity = EntityManager.CreateEntity(typeof(Nest1));
        ////EntityManager.SetComponentData(entity, new Nest1
        ////{
        ////    nestStatus = NestStatus.Two,
        ////    nestStatusCollection = new NestArrayStatusCollection { Value = 5 }
        ////});

        //EntityManager.SetComponentData(entity, new Nest1
        //{
        //    nestStatus = NestStatus.Two,
        //    nestStatusCollection = new NestArrayStatusCollection { Value = new NativeArray<int>(new int[3] { 1,2,3},Allocator.Temp) }
        //});



        //BufferFromEntity<NestArrayStatusCollection> lookup = GetBufferFromEntity<NestArrayStatusCollection>();
        //var buffer = lookup[entity];
        //buffer.Add(17);


        var nest2 = EntityManager.CreateEntity();
        EntityManager.AddBuffer<NestStatusCollection>(nest2);

        BufferFromEntity<NestStatusCollection> lookup = GetBufferFromEntity<NestStatusCollection>();
        var buffer = lookup[nest2];
        buffer.Add(17);
        buffer.Add(52);

    }
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref Nest1 nest1) =>
        {
            Debug.Log("Get The Entity!");
            

        }).Run();

    }
}
