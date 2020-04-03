using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerToEntity : IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        EntityArchetype playerArchetype = dstManager.CreateArchetype();


        dstManager.AddComponent(entity, );
    }
}
