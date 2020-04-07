using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Vic
//This class works as a data container for the "floor". It is used for floor tiles which are important for the player's movement, therefore it is not suitable for decoration
[ExecuteInEditMode]
public class ProGridTile : MonoBehaviour
{
    //Player starts on this tile
    public bool startingTile = false;
    //Player ends level on this tile
    public bool endingTile = false;

    public bool teleportationTile = false;

    public bool damageTile = false;

    public bool pushTile = false; 

    public float cellSize = 1;

    [RangeAttribute(0.1f, 1)]
    public float scaleFactor = 1;

    MeshRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Setting tag for this object. Floor tiles will have the same
        gameObject.tag = GameObject.Find("GameController").GetComponent<Game>().getTileTag();

        mRenderer = gameObject.GetComponent<MeshRenderer>();

        //If there is no MeshRenderer on the object, it may exists in its child
        if (mRenderer == null)
        {
            mRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

            if (mRenderer == null)
            {
                Debug.LogError("Mesh Renderer Missing!");
            }
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
            snapToGrid();

    }

    private void snapToGrid()
    {
        if (cellSize == 0) cellSize = 1;

        float gridX;
        float gridZ;

        gridX = Mathf.Round(transform.position.x / cellSize) * cellSize;
        gridZ = Mathf.Round(transform.position.z / cellSize) * cellSize;

        float gridY = mRenderer.bounds.size.y / 2;
        transform.position = new Vector3(gridX, gridY, gridZ);

        scaletoFit();
    }

    //Checking if grid position x-z is next to this tile coordinates in Game Class. This is done by comparing to its own coordinates
    public bool movable(float x, float z)
    {

        //Debug.Log(x+" y "+z +" "+transform.position.x+" y "+transform.position.z);
        //Approximation, numbers may not be exact
        float tolerance = 0.2f;
        if (Mathf.Abs(x - transform.position.x) > tolerance) return false;
        if (Mathf.Abs(z - transform.position.z) > tolerance) return false;

        return true;

    }


    void scaletoFit()
    {
        float xSize = mRenderer.bounds.size.x;
        float zSize = mRenderer.bounds.size.z;

        Vector3 actualScale = gameObject.transform.localScale;

        Vector3 scaleVector = new Vector3(actualScale.x * scaleFactor * cellSize / xSize, actualScale.y, actualScale.z * scaleFactor * cellSize / zSize);

        gameObject.transform.localScale = scaleVector;
    }

}
