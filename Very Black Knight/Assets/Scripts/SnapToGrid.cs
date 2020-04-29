using UnityEngine;

//Author: Vic
[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    public float cellSize = 1;
    public bool placeOnGrid = false;
    public bool touchingFloor = true;


    public bool scaleXToFitCell = false;
    public bool scaleZToFitCell = false;

    public bool ignoreSnapX = false;
    public bool ignoreSnapZ = false;

    [RangeAttribute(0.1f, 1)]
    public float scaleFactor = 1;

    public Vector2 dimensions;

    MeshRenderer mRenderer;

    void Start()
    {
        mRenderer = gameObject.GetComponent<MeshRenderer>();

        //If there is no MeshRenderer on the object, it may exists in its child
        if (mRenderer == null)
        {
            mRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

            if (mRenderer == null)
            {
                Debug.LogError("Mesh Renderer Missing in :"+gameObject.name+" !");
            }
        }



    }
    // Update is called once per frame
    void Update()
    {
        //Execute ONLY on edit Mode
        if (!Application.isPlaying)
        {
            if (dimensions.x == 0) dimensions.x = 1;
            if (dimensions.y == 0) dimensions.y = 1;
            if (cellSize == 0) cellSize = 1;


            if (placeOnGrid)
            {
                if (scaleXToFitCell) scaleXtoFit();
                if (scaleZToFitCell) scaleZtoFit();

                float gridX;
                float gridZ;

                if (!ignoreSnapX)
                    //Place on X grid
                    gridX = Mathf.Round(transform.position.x / cellSize) * cellSize;
                else
                {
                    gridX = transform.position.x;
                }

                if (!ignoreSnapZ)
                    //Place on Y grid
                    gridZ = Mathf.Round(transform.position.z / cellSize) * cellSize;
                else
                {
                    gridZ = transform.position.z;
                }


                if (!touchingFloor)
                {
                    transform.position = new Vector3(gridX, transform.position.y, gridZ);
                }
                else
                {
                    //Get model's height/2 
                    float gridY = mRenderer.bounds.size.y / 2;
                    //Place Object so it is touching ground perfectly
                    transform.position = new Vector3(gridX, gridY, gridZ);

                }
            }
        }
    }

    void scaleXtoFit()
    {
        //Get model's x size
        float xSize = mRenderer.bounds.size.x;
        Vector3 actualScale = gameObject.transform.localScale;

        //Calculate needed size
        Vector3 scaleVector = new Vector3(actualScale.x * scaleFactor * cellSize * dimensions.x / xSize, actualScale.y, actualScale.z);

        //Apply scale to model
        gameObject.transform.localScale = scaleVector;
    }

    void scaleZtoFit()
    {
        //Get model's z size
        float zSize = mRenderer.bounds.size.z;
        Vector3 actualScale = gameObject.transform.localScale;

        //Calculate needed size
        Vector3 scaleVector = new Vector3(actualScale.x, actualScale.y, actualScale.z * scaleFactor * cellSize * dimensions.y / zSize);

        //Apply scale to model
        gameObject.transform.localScale = scaleVector;
    }

}
