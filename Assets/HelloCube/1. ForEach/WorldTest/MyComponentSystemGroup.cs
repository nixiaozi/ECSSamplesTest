
using Unity.Entities;
using UnityEngine.UIElements;

public class MyComponentSystemGroup: ComponentSystemGroup
{

    protected override void OnUpdate()  // 可以重写默认的OnUpdate方法    
    {
        // someone on the forum said he wanted to update some system again if some later system did something, 
        // I imagine this would be the right place to do.


        base.OnUpdate();
    }



    public override void SortSystemUpdateList()
    {
        // 这里可以重写System的排序方法
        // Maybe you could call base.SortSystemUpdateList for [UpdateBefore/After] sorting first then do your own business afterwards.

        base.SortSystemUpdateList();
    }

}

