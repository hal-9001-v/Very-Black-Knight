using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour

{
    public GameObject targetContainerObject;

    Camera myCamera;
    Light myLight;
    Transform target;

    Vector3 cameraOffset;
    Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = gameObject.GetComponent<Camera>();
        myLight = gameObject.GetComponent<Light>();

        target = targetContainerObject.GetComponent<Transform>();


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

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        destination = target.transform.position + cameraOffset;

        //transform.LookAt(Vector3.Lerp(transform.rotation.eulerAngles,target.position,1));
        //transform.LookAt(target);

        if (Vector3.Distance(destination, transform.position) > 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);

        }
    }

}
