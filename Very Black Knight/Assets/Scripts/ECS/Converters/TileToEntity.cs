using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class TileToEntity : MonoBehaviour,IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity,typeof(TileComponent));

        dstManager.AddComponentData<TileComponent>(entity, new TileComponent
        {
            xPosition = 0,
            zPosition = 1
        }) ;


    }
}
