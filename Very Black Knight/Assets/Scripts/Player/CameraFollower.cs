using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject playerObject;
    private PlayerMovement movementScript;

    Transform target;
    //Light myLight;

    Vector3 cameraOffset;
    Vector3 destination;


    [Range(0.1f, 5)]
    public float speedFactor = 1;
    // Start is called before the first frame update
    void Start()
    {
        //myLight = gameObject.GetComponent<Light>();

        target = playerObject.GetComponent<Transform>();
        movementScript = playerObject.GetComponent<PlayerMovement>();

        //Locked position through player
        cameraOffset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        //Rotate camera 90 degrees with player as axis 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            //New ofset is calculated with a rotation
            cameraOffset = rotation * cameraOffset;
            destination = target.transform.position + cameraOffset;
            //Smooth Transition
            movementScript.setNewDirections(rotation);
        }else
        //Rotate camera -90 degrees with player as axis 
        if (Input.GetKeyDown(KeyCode.E))
        {
            Quaternion rotation = Quaternion.Euler(0,  270, 0);
            //New ofset is calculated with a rotation
            cameraOffset = rotation * cameraOffset;
            //Smooth Transition
            destination = target.transform.position + cameraOffset;


            //Updating controllers
            movementScript.setNewDirections(rotation);

        }

        //Moves on camera are interpolations. Destination variable is the "locked" place for the camera
        destination = target.transform.position + cameraOffset;

           //Smooth Look At
            Quaternion smoothRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, smoothRotation, speedFactor * Time.deltaTime);
        

        //If camera is close enough to destination, interpolation stops
        if (Vector3.Distance(destination, transform.position) > 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);

        }
        
    }

}
