using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[ExecuteInEditMode]
public class TileToEntity : MonoBehaviour, IConvertGameObjectToEntity
{

    private float tileSpacing;
    
    [SerializeField]
    public bool startOnThisTile;

    [RangeAttribute(0.1f, 1)]
    public float scaleFactor = 1;

    Renderer mr;

    void Start()
    {
        mr = gameObject.GetComponent<Renderer>();

        //If there is no MeshRenderer on the object, it may exists in its child
        if (mr == null)
        {
            mr = gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();

            if (mr == null)
            {
                Debug.LogError("Mesh Renderer Missing!");
            }
        }

        tileSpacing = 1;

        MeshFilter ms = gameObject.GetComponent<MeshFilter>();
        

    }

    void Update()
    {
#if UNITY_EDITOR

        float gridX;
        float gridZ;
        float gridY;

        gridX = Mathf.Round(transform.position.x / tileSpacing) * tileSpacing;
        gridY = mr.bounds.size.y / 2;
        gridZ = Mathf.Round(transform.position.z / tileSpacing) * tileSpacing;

        transform.position = new Vector3(gridX, gridY, gridZ);
        gameObject.SetActive(true);
        
        scaleToFit();
        
#endif
    }


    void scaleToFit()
    {
        float xSize = mr.bounds.size.x;
        float zSize = mr.bounds.size.z;

        if (xSize == 0 || zSize == 0)
        {
            Debug.LogWarning("Bounds are 0");
            return;
        }
        Vector3 actualScale = gameObject.transform.localScale;


        Vector3 scaleVector = new Vector3(actualScale.x * scaleFactor * tileSpacing / xSize,
            actualScale.y,
            actualScale.z * scaleFactor * tileSpacing / zSize);

        gameObject.transform.localScale = scaleVector;
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        dstManager.AddComponent(entity, typeof(TileComponent));

        dstManager.AddComponentData<TileComponent>(entity, new TileComponent
        {
            position = new float2(gameObject.transform.position.x, gameObject.transform.position.z),
            startingTile = startOnThisTile,
            spacing = tileSpacing
        });
    }
}
