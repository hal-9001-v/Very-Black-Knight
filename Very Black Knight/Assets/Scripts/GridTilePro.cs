using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Vic
//This class works as a data container for the "floor". It is used for floor tiles which are important for the player's movement, therefore it is not suitable for decoration
public class GridTilePro : MonoBehaviour
{
    //Player starts on this tile
    public bool startingTile = false;
    //Player ends level on this tile
    public bool endingTile = false;

    public bool hurtTile = false;
    
    [SerializeField]
    public float damage = 1;
    
    public bool teleportationTile = false;
    public GameObject teleportationTileObject;
    

    public bool pushTile = false;
    public GameObject pushTileObject;

    public float cellSize = 1;

    
    [RangeAttribute(0.1f, 1)]
    public float scaleFactor = 1;

    MeshRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Setting tag for this object. Floor tiles will have the same
        gameObject.tag = GameObject.Find("GameController").GetComponent<Game>().tileTag;

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


    



    // Update is called once per frame
    void Update()
    {
        if (cellSize == 0) cellSize = 1;


        scaleToFit();

        float gridX;
        float gridZ;
        float gridY;

        gridX = Mathf.Round(transform.position.x / cellSize) * cellSize;
        gridY = mRenderer.bounds.size.y / 2;
        gridZ = Mathf.Round(transform.position.z / cellSize) * cellSize;
        transform.position = new Vector3(gridX, gridY, gridZ);

    }


    void scaleToFit()
    {
        float xSize = mRenderer.bounds.size.x;
        float zSize = mRenderer.bounds.size.z;
        Vector3 actualScale = gameObject.transform.localScale;

        Vector3 scaleVector = new Vector3(actualScale.x * scaleFactor * cellSize / xSize, actualScale.y, actualScale.z * scaleFactor * cellSize / zSize);

        gameObject.transform.localScale = scaleVector;
    }

}
