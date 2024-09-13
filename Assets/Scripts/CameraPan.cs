using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{

    Camera camera;

    Vector2 mouseLastPosition;
    Vector2 mousePosDelta;

    public float panSpeed = 0.01f;
    bool isPanActive = false;

    // Start is called before the first frame update
    void Start()
    {
        panSpeed = 0.01f;
        camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isPanActive = true;
            mouseLastPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(2))
        {
            isPanActive = false;
        }

        if (isPanActive == true)
        {
            mousePosDelta = (Vector2)camera.ScreenToWorldPoint(Input.mousePosition) - mouseLastPosition;
            camera.transform.position += (Vector3)mousePosDelta * panSpeed;
        }

    }
}
