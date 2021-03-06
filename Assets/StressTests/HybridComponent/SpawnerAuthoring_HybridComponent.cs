﻿using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

// ReSharper disable once InconsistentNaming
[RequiresEntityConversion]
[AddComponentMenu("DOTS Samples/HybridComponent/Spawner")]  // 这个是添加了一个组件菜单可以直接使用UI点击添加组件
[ConverterVersion("joe", 1)]
public class SpawnerAuthoring_HybridComponent : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject prefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Spawner_HybridComponent
        {
            prefab = conversionSystem.GetPrimaryEntity(prefab)
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }
}
