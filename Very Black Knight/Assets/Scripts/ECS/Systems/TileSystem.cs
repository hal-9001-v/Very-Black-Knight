using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class TileSystem : ComponentSystem
{
    protected override void OnUpdate(){
        Entities.ForEach((ref TileComponent tileComponent) => {
  
        });
    }
}
