using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class PlayerToEntity : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(PlayerComponent));

        
        dstManager.AddComponentData(entity, new PlayerComponent
        {
            health = 1,
            speed = 1

        });

        dstManager.AddComponent(entity, typeof(Translation));

    }
}
