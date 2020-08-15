

using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevel;

[UpdateAfter(typeof(TestSystem_ForEach))]
public class WorldTest_ForEach : SystemBase
{

    protected override void OnCreate()
    {
        /*// 这个是手动添加System到当前World的方法（默认是Default World）
        World.GetOrCreateSystem(typeof(TestSystem_ForEach)); // 这个是如果存在就直接获取，如果不存在就创建
        World.CreateSystem(typeof(TestSystem_ForEach)); //重复创建System会怎样？ 重复创建导致该system的OnCreate()方法重复执行
        World.AddSystem<TestSystem_ForEach>(new TestSystem_ForEach()); // 也是天价system到world的方法，效果同CreateSystem
        World.AddSystem<TestSystem_ForEach>(new TestSystem_ForEach()); // 重复执行相同方法会有什么问题？ 会重新执行以下OnCreate方法
*/

        /*
        ComponentSystemGroup componentSystemGroup = new ComponentSystemGroup; // ComponentSystemGroup 是抽象类，无法直接实例化
        ComponentSystemGroup.AddSystemToUpdateList(new TestSystem_ForEach());
        */

      /*  
        MyComponentSystemGroup componentSystemGroup = new MyComponentSystemGroup();
        componentSystemGroup.AddSystemToUpdateList(new TestSystem_ForEach());
        World.AddSystem<MyComponentSystemGroup>(componentSystemGroup);  // 这个就是World添加ComponentSystemGroup的正确方法，但是这个好像并没有调用OnCreate方法？
*/

        /*componentSystemGroup.SortSystemUpdateList();  // 这个是通过 [UpdateBefore/UpdateAfter] 属性标签自动对更新顺序进行调整


        componentSystemGroup.Update();  // 这个方法是显式的更细当前的componentSystemGroup行为集*/

/*
        // using UnityEngine.Experimental.PlayerLoop;
        var invisiblehand= PlayerLoop.GetDefaultPlayerLoop();   // 通过这个方法你可以找到调度不同system的Onupdate方法的看不见的手。
        var subSystemList = invisiblehand.subSystemList; // 列出每个游戏循环需要调用的system列表
        for(var i = 0; i < subSystemList.Length; i++)
        {
            // subSystemList[i].updateDelegate = CustomOnUpdate;  这个可以自定义每个system的update方法
        }
*/


/*
        //ScriptBehaviourUpdateOrder.UpdatePlayerLoop(world)
        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World); //  这个方法是初始化PlayerLoop 定义最高级别的systemgroup加载顺序
        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(null); // to reset the player loop to the default classic state
*/


/*
        // 这个是ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World); 这个行为默认定义的systemgroup加载顺序，所加载的group
        InitializationSystemGroup initializationSystemGroup = new InitializationSystemGroup();
        SimulationSystemGroup simulationSystemGroup = new SimulationSystemGroup();
        PresentationSystemGroup presentationSystemGroup = new PresentationSystemGroup();
*/




    }


    private void CustomOnUpdate()
    {

    }


    protected override void OnUpdate()
    {
        Debug.Log("WorldTest可以先执行么？");
    }
}