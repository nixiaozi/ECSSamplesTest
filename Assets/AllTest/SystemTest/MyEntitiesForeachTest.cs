

using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace AllTest.SystemTest
{

    public class MyEntitiesForeachTest : SystemBase
    {
        private EntityQuery query;
        protected override void OnCreate()
        {
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A1), typeof(A2),typeof(A3)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A1), typeof(A2)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A1), typeof(A3)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A2), typeof(A3)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A1)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A2)), 5, Allocator.Temp);
            EntityManager.CreateEntity(EntityManager.CreateArchetype(typeof(A3)), 5, Allocator.Temp);

/*          // 实例是用来获取Entities.ForEach()方法及其扩展获得的EntityQuery 所以这里不需要初始化
            var queryDesc = new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(A3) },
                Any = new ComponentType[] { typeof(A1),typeof(A2) }
            };
            query = GetEntityQuery(queryDesc);  // EntityQuery 初始化
*/

        }

        protected override void OnUpdate()
        {
            int dataCount = query.CalculateEntityCount(); // 获取到底有多少个实体
            NativeArray<float> dataSquared
                        = new NativeArray<float>(dataCount, Allocator.TempJob); // 分配在工作中的缓存需要使用TempJob
            var job1Handle = Entities
                .WithName("DefaultEntityQuery")
                .WithStoreEntityQueryInField(ref query) // 这个是把Entities.Foreach()以及它的扩展方法获得的EntityQuery 传递给query
                // .WithAny<A1,A3>()
                .ForEach((int entityInQueryIndex
                    //,ref A1 data1
                    ,ref A2 data2
                    ,ref A3 data3
                    ) =>
                {
                    dataSquared[entityInQueryIndex] = entityInQueryIndex;
                    // Debug.Log("这是第" + entityInQueryIndex + "个含有A1组件的实体");  // A1 是可选组件，看看会不会有问题
                    // Burst 编译不支持上面的格式需要使用:
                    Debug.Log(string.Format("这是第{0}个含有A1组件的实体", entityInQueryIndex)); // index 最小为零
                })
                // .ScheduleParallel();
                .Schedule(Dependency);  // 自定义依赖 需要添加这个参数 Dependency


            var job2Handle = Job
                .WithName("Then_Job")   //  不能是Then Job 这样分开的单词
                .WithCode(() =>
            {
                for (int i = 0; i < dataSquared.Length; i++)
                {
                    dataSquared[0] += dataSquared[i];
                }
                // dataSquared.Dispose(); // error : 
                //dataSquared.Dispose(); // error:InvalidOperationException: The native container may not be deallocated on a job.
                Debug.Log("我还有什么要做的呢？");
            })
                // WithDeallocateOnJobCompletion can only be used on variables that are captured in your Entities.ForEach lambda. 
                // .WithDeallocateOnJobCompletion(dataSquared) // 工作完成后解除分配    文档中是 .WithDisposeOnCompletion(result)
                .Schedule(job1Handle);

            Dependency = dataSquared.Dispose(job2Handle); // dataSquared.Dispose  在 job2Handle 之后执行，并且把依赖返回给整个system对象


            // query.Dispose(); // error:InvalidOperationException: EntityQuery cannot currently be disposed
            // dataSquared.Dispose(); // 非托管类型变量需要手动销毁
        }

        protected override void OnDestroy()
        {
            // query.Dispose();
        }

    }


    public struct A1:IComponentData
    {

    }

    public struct A2 : IComponentData { }

    public struct A3 : IComponentData { }

}



