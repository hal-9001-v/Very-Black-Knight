using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Vic
//This class works as a data container for the "floor". It is used for floor tiles which are important for the player's movement, therefore it is not suitable for decoration
public class GridTile : MonoBehaviour
{
    //Player starts on this tile
    public bool startingTile = false;
    //Player ends level on this tile
    public bool endingTile = false;

    // Start is called before the first frame update
    void Start()
    {
        //Setting tag for this object. Floor tiles will have the same
        gameObject.tag = GameObject.Find("GameController").GetComponent<Game>().getTileTag();
    }

    //Checking if grid position x-z is next to this tile coordinates in Game Class. This is done by comparing to its own coordinates
    public bool movable(float x, float z) {

        //Debug.Log(x+" y "+z +" "+transform.position.x+" y "+transform.position.z);
        //Approximation, numbers may not be exact
        float tolerance = 0.2f;
        if (Mathf.Abs(x - transform.position.x) > tolerance) return false;
        if (Mathf.Abs(z - transform.position.z) > tolerance) return false;

        return true;
    
    }

  
}
