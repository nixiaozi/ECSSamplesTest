using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

/*
 1、为什么要添加 [RequiresEntityConversion]
这个只是用来在你忘记添加 ConvertToEntity  脚本是给你一个提示告警
 
2、为什么会有AddComponentMenu 
这是是为脚本添加一个自定义的名字，这样在Entities选择的时候可以使用这个名字，自定义的名字也相对好记

3、为什么会有ConverterVersion 
这个只是一个你自己自定义的版本号，如果你修改了版本号，Unity就知道版本变更了一定要重新编译才行
 
 */


// ReSharper disable once InconsistentNaming
[RequiresEntityConversion]
[AddComponentMenu("DOTS Samples/SpawnFromEntity/Spawner")]
[ConverterVersion("joe", 1)]
public class SpawnerAuthoring_FromEntity : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject Prefab;
    public int CountX;
    public int CountY;

    // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(Prefab);
    }

    // Lets you convert the editor data representation to the entity optimal runtime representation
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new Spawner_FromEntity
        {
            // The referenced prefab will be converted due to DeclareReferencedPrefabs.
            // So here we simply map the game object to an entity reference to that prefab.
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            CountX = CountX,
            CountY = CountY
        };
        dstManager.AddComponentData(entity, spawnerData);
    }
}
