
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


public class TestSystem_ForEach : SystemBase
{
    /*private NativeArray<Entity> result2;
    private NativeArray<C3> result2C3;*/

    private NativeArray<Entity> entities5;
    private NativeArray<Entity> entities6;
    private NativeArray<Entity> entities7;
    private NativeArray<Entity> entities8;
    private NativeArray<Entity> entities9;
    private NativeArray<Entity> entities10;
    private NativeArray<Entity> entities11;

    // private EntityQuery EntityQueryTest;

    private EntityQuery m_Query_2; // EntityQuery 会自动被系统回收
    private EntityQuery m_Query_1;
    private EntityQuery m_Query_0;

    private EntityArchetype archetype;

    protected override void OnCreate()
    {
        // 首先需要创建实体，测试用
        archetype = EntityManager.CreateArchetype(typeof(C1), typeof(C2), typeof(C3));
        entities5 = EntityManager.CreateEntity(archetype, 5, Allocator.Temp);

        archetype = EntityManager.CreateArchetype(typeof(C1), typeof(C2));
        entities6 = EntityManager.CreateEntity(archetype, 6, Allocator.Temp);

        archetype = EntityManager.CreateArchetype(typeof(C1), typeof(C3));
        entities7 = EntityManager.CreateEntity(archetype, 7, Allocator.Temp);  

        archetype = EntityManager.CreateArchetype(typeof(C2), typeof(C3));
        entities8 = EntityManager.CreateEntity(archetype, 8, Allocator.Temp);

        archetype = EntityManager.CreateArchetype(typeof(C1));
        entities9 = EntityManager.CreateEntity(archetype, 9, Allocator.Temp);

        archetype = EntityManager.CreateArchetype(typeof(C2));
        entities10 = EntityManager.CreateEntity(archetype, 10, Allocator.Temp);

        archetype = EntityManager.CreateArchetype(typeof(C3));
        entities11 = EntityManager.CreateEntity(archetype, 11, Allocator.Temp);

        //var queryDescription = new EntityQueryDesc
        //{
        //    All = new ComponentType[] {
        //      ComponentType.ReadWrite<W>(),
        //      ComponentType.ReadOnly<B>()
        //   },
        //    Options = EntityQueryOptions.FilterWriteGroup
        //};
        //EntityQueryTest = GetEntityQuery(queryDescription);

        var query_0 = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(C1), ComponentType.ReadOnly<C3>() },
            Options = EntityQueryOptions.FilterWriteGroup  // 文档中使用的 EntityQueryDescOptions.FilterWriteGroup 是错误的，不存在枚举EntityQueryDescOptions
        };
        m_Query_2 = GetEntityQuery(query_0);
    }


    protected override void OnUpdate()
    {


        // 第一种获取 EntityQuery 的方法。
        m_Query_0 = GetEntityQuery(typeof(EntityAge_ForEach),
                            ComponentType.ReadOnly<EntityCount_ForEach>());    // 这个是定义了一个 EntityQuery 查询

        // 这个可以直接使用lamb表达式形式的多重筛选，第二种获取 EntityQuery 的方法。
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(C1), typeof(C2) },
            All = new ComponentType[]{ typeof(EntityAge_ForEach),
                                           ComponentType.ReadOnly<EntityCount_ForEach>() }
        };
        m_Query_1 = GetEntityQuery(query);


        // 
        // ... In a system:
        /*var query_0 = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(C1), ComponentType.ReadOnly<C3>() },
            Options =  EntityQueryOptions.FilterWriteGroup  // 文档中使用的 EntityQueryDescOptions.FilterWriteGroup 是错误的，不存在枚举EntityQueryDescOptions
        };
        m_Query_2 = GetEntityQuery(query);*/  // 这里似乎应该把query 改为 query_0


        // var result2= m_Query_2.ToEntityArray(Allocator.TempJob);  //  这个方法意义从EntityQuery获得NativeArray<Entity> 的集合
        NativeArray<Entity>  result2 = m_Query_2.ToEntityArray(Allocator.TempJob); // 上面直接在方法内定义非托管变量，会导致result2在方法完成后失去变量指针，变得没有办法管理（内存泄漏）
        NativeArray<C3>  result2C3 = m_Query_2.ToComponentDataArray<C3>(Allocator.TempJob); // m_Query_2.ToComponentDataArray<C1>() 不带参数的方法必须保证C1是引用类型如class
        // 上面两个方法会造成内存泄漏

        for (int i = 0; i != result2C3.Length; i++)
        {
            //Random random = new Random(i);
            //C3 c3 = new C3 { Long = (float)(result2C3[i].Long + (random.NextDouble() * 2.0) - 1.0) };
            //result2C3[i] = c3;

            Debug.Log("这时候有一个Component");
        }


        /*EntityQueryTest.ToEntityArray(Allocator.Temp).ForEach((ref W w, in B b) => {
            // perform computation here
        }).ScheduleParallel();*/


        result2.Dispose();
        result2C3.Dispose();

    }


    protected override void OnDestroy()
    {
        // result2.Dispose();  在Onupdate方法内使用的变量需要在每次执行完成方法的内部Dispose（）,或者使用局部变量方法完成后自动销毁
        // result2C3.Dispose();
/*        m_Query_2.Dispose();
        m_Query_1.Dispose();  // 这个好像也不需要在最后Dispose()
        m_Query_0.Dispose();*/

        // m_Query_1.Dispose(); // The EntityQuery will automatically be disposed by the ComponentSystem.
        // m_Query_0.Dispose(); // 原因同上

/*        entities5.Dispose();
        entities6.Dispose();  // 出现错误 The NativeArray has been deallocated, it is not allowed to access it
        entities7.Dispose();
        entities8.Dispose();
        entities9.Dispose();
        entities10.Dispose();
        entities11.Dispose();*/

        
    }

}

[GenerateAuthoringComponent]
struct SharedGrouping : ISharedComponentData
{
    public int Group;
}



[GenerateAuthoringComponent]
[WriteGroup(typeof(C1))]
public struct C3 : IComponentData 
{ 
    public int Long { get; set; } 
} // 不能添加初始值设定



/*
 * 
 * 下面的实例来自于 https://docs.unity3d.com/Packages/com.unity.entities@0.13/manual/ecs_write_groups.html
 * 需要用来对出现的EntityQuery没有声明组件的错误进行调试
 * 
 */

[GenerateAuthoringComponent]
public struct W : IComponentData
{
    public int Value;
}

[GenerateAuthoringComponent]
[WriteGroup(typeof(W))]
public struct A : IComponentData
{
    public int Value;
}

[WriteGroup(typeof(W))]
[GenerateAuthoringComponent]
public struct B : IComponentData
{
    public int Value;
}
