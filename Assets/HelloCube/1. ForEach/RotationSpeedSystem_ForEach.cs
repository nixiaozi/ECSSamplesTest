using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// This system updates all entities in the scene with both a RotationSpeed_ForEach and Rotation component.

// ReSharper disable once InconsistentNaming
public class RotationSpeedSystem_ForEach : SystemBase
{
    // OnUpdate runs on the main thread.
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        // Schedule job to rotate around up vector
        Entities
            .WithName("RotationSpeedSystem_ForEach")
            .ForEach((ref Rotation rotation, in RotationSpeed_ForEach rotationSpeed) =>  // RotationSpeed_ForEach  这个是输入参数，Rotation 是输出参数改变量
            {
                rotation.Value = math.mul(
                    math.normalize(rotation.Value), 
                    quaternion.AxisAngle(math.up(), rotationSpeed.RadiansPerSecond * deltaTime));  // 获取当前角度，并且修改当前角度
            })
            .ScheduleParallel();
    }
}
