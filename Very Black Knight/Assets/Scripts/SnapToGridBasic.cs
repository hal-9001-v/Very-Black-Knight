using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Author: Vic
[ExecuteInEditMode]
public class SnapToGridBasic : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            float gridX;
            float gridZ;

                gridX = Mathf.Round(transform.position.x);
                gridZ = Mathf.Round(transform.position.z);
            
            transform.position = new Vector3(gridX, transform.position.y, gridZ);

        }
    }
}


