


using Unity.Entities;

public class CustomBootup : ICustomBootstrap
{
    public bool Initialize(string defaultWorldName)
    {
        // 这里可以添加自定义的初始化代码

        return true;
    }
}

