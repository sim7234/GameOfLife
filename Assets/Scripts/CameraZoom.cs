using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    new Camera camera;

    float minZoom = 1f;
    float maxZoom = 1000f;

    public float zoomSpeed = 50f;
    public float zoomSpeedAcceleration = 5f;

    float targetZoomSpeed;
    float maxZoomSpeed = 200;
    float minZoomSpeed = 5f;

    float scroll;

    public float whenToSlow = 30f;

    // Start is called before the first frame update
    void Start()
    {
        if (zoomSpeed > maxZoomSpeed)
        {
            zoomSpeed = maxZoomSpeed;
        }
        targetZoomSpeed = zoomSpeed;

        
       // zoomSpeed = 50f;
        minZoom = 1f;
        maxZoom = 500f;
        camera = Camera.main;
       // zoomSpeedAcceleration = 5f;

        maxZoomSpeed = 200f;
        minZoomSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (zoomSpeed > maxZoomSpeed)
        {
            zoomSpeed = maxZoomSpeed;
        }



        scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (camera.orthographicSize < whenToSlow)
            {
                zoomSpeed -= zoomSpeedAcceleration;
            }
            else if (zoomSpeed < targetZoomSpeed)
            {
                zoomSpeed += zoomSpeedAcceleration;
            }


            zoomSpeed = Mathf.Clamp(zoomSpeed, minZoomSpeed, maxZoomSpeed);
            camera.orthographicSize -= (scroll * zoomSpeed);
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        }
    }
}
