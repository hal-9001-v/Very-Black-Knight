using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour

{
    public GameObject targetContainerObject;

    Camera myCamera;
    Transform target;
    //Light myLight;

    Vector3 cameraOffset;
    Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = gameObject.GetComponent<Camera>();
        //myLight = gameObject.GetComponent<Light>();

        target = targetContainerObject.GetComponent<Transform>();

        //Locked position through player
        cameraOffset = transform.position - target.transform.position;

        if (myCamera == null)
        {
            Debug.LogError("Camera is missing on object!");
        }

        if (target == null)
        {
            Debug.LogError("Target Reference is missing!");
        }

    }

    void LateUpdate()
    {
        //Moves on camera are interpolations. Destination variable is the "locked" place for the camera
        destination = target.transform.position + cameraOffset;

        //If camera is close enough to destination, interpolation stops
        if (Vector3.Distance(destination, transform.position) > 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);

        }
    }

}
