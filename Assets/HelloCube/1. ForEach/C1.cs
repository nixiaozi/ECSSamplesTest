using Unity.Entities;

[GenerateAuthoringComponent]
public struct C1 : IComponentData 
{
    public int Long { get; set; } // = 0f; 不能添加初始值设定
}
