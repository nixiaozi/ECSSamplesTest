
using Unity.Entities;

public struct Nest2 :IComponentData
{
    public NestStatus nestStatus;

    public NestStatusCollection nestStatusCollection;

}

public struct NestStatusCollection : IBufferElementData
{
    // Actual value each buffer element will store.
    public int Value;

    // The following implicit conversions are optional, but can be convenient.
    public static implicit operator int(NestStatusCollection e)
    {
        return e.Value;
    }

    public static implicit operator NestStatusCollection(int e)
    {
        return new NestStatusCollection { Value = e };
    }
}