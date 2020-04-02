using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject axisObject;
    public float distanceToAxis = 1;
    public string tileTag = "tile";
    public bool keepVerticality = true;
    GameObject endingTileObject;

    // Start is called before the first frame update
    void Start()
    {
        if (axisObject == null) {
            Debug.LogError("Axis Object for compass is missing!");
        }

        setDestination();
    }

    // Update is called once per frame
    void Update()
    {
        updateCompass();
        
    }

    
    private void updateCompass() {
        //Locate on axis
        transform.position = axisObject.transform.position;

        //Rotate to the point. Local Z axis of the object points at target
        transform.LookAt(endingTileObject.transform);

        //Calculate direction toward the ending tile in order to move compass in that direction
        Vector3 dir = Vector3.Normalize(endingTileObject.transform.position - transform.position);

        //Amount of distance from the player
        dir *= distanceToAxis;


        //Axis position + "local" distance to it
        transform.position += dir;

    }

    private void setDestination() {
        
        GameObject[] goList = GameObject.FindGameObjectsWithTag(tileTag);

        foreach (GameObject go in goList)
        {
            if (go.GetComponent<GridTile>().endingTile)
            {
                endingTileObject = go;
            }

        }
    }
}
