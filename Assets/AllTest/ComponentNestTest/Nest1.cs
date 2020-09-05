
using Unity.Collections;
using Unity.Entities;

public struct Nest1 :IComponentData
{
    public NestStatus nestStatus;

    // public NestArrayStatusCollection nestStatusCollection; // commit for below error:
    // Nest1 contains a field of Unity.Collections.LowLevel.Unsafe.DisposeSentinel, which is neither primitive nor blittable.
}



public struct NestArrayStatusCollection : IBufferElementData
{
    // Actual value each buffer element will store.
    public NativeArray<int> Value;

    // The following implicit conversions are optional, but can be convenient.
    public static implicit operator NativeArray<int>(NestArrayStatusCollection e)
    {
        return e.Value;
    }

    public static implicit operator NestArrayStatusCollection(NativeArray<int> e)
    {
        return new NestArrayStatusCollection { Value = e };
    }
}



public enum NestStatus
{
    One,
    Two
}