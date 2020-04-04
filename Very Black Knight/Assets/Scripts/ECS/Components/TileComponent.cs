using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public struct TileComponent : IComponentData
{
    public float2 position;
    public bool startingTile;
    public float spacing;
}
