using Unity.Entities;

[GenerateAuthoringComponent] // 没有这个可能出现错误Trying to get iterator for C3 but the required component type was not declared in the EntityQuery.
[WriteGroup(typeof(C1))]  //  原式中的 [WriteGroup(C1)] 会出现错误C1不是类型，所以需要添加 typeof
public struct C2 : IComponentData 
{ 
    public int Long { get; set; } 
}