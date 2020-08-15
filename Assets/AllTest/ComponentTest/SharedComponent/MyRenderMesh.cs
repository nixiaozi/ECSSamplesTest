using System;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 由于未解决 托管类型 Mesh 和 Material 在这个造成的问题，这个组件先搁置使用
/// </summary>

[System.Serializable]
public struct MyRenderMesh : ISharedComponentData, IEquatable<Mesh>,IEquatable<Material>
{
/*
    public Mesh mesh;
    public Material material;
*/
    public ShadowCastingMode castShadows;
    public bool receiveShadows;

    public bool Equals(Mesh other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(Material other)
    {
        throw new NotImplementedException();
    }
}