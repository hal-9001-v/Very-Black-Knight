using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct PlayerComponent : IComponentData
{
    public float3 position;
    public float health;
    public float speed;

}
