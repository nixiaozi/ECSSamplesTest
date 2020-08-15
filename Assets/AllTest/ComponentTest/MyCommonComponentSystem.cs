
using System.Runtime.InteropServices.ComTypes;
using Unity.Entities;

public class MyCommonComponentSystem : SystemBase
{

    protected override void OnCreate()
    {
        // 创建带有sharedComponent 的实体
        // EntityManager.CreateEntity(typeof(MyRenderMesh));  // 暂时先搁置





    }



    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("MyCommonComponent")
            .ForEach((ref MyCommonComponent my) =>
            {
                my.AgeRank += deltaTime;
            })
            .ScheduleParallel();



    }
}

