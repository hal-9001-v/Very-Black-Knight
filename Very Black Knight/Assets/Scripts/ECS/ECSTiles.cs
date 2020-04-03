using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
public class ECSTiles : MonoBehaviour
{
    [SerializeField]
    private int xSize;

    [SerializeField]
    private int ySize;

    [SerializeField]
    private float spacing;

    [SerializeField]
    private Mesh tileMesh;
    [SerializeField]
    private Material tileMaterial;

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype tileArchetype = entityManager.CreateArchetype(
                typeof(TileComponent),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld)

            );

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                Entity myEntity = entityManager.CreateEntity(tileArchetype);

                entityManager.SetComponentData(myEntity,
                    new TileComponent
                    {
                        xPosition = i * spacing,
                        zPosition = j * spacing
                    });

                entityManager.SetComponentData(myEntity,
                    new Translation
                    {
                        Value = new float3(i * spacing, 0, j * spacing)
                    });

                entityManager.SetSharedComponentData(myEntity,
                    new RenderMesh
                    {
                        mesh = tileMesh,
                        material = tileMaterial
                    });

            }
        }

    }


}
