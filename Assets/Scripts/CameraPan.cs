using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{

    new Camera camera;

    Vector2 mouseLastPosition;
    Vector2 mousePosDelta;

    public float panSpeed = 0.01f;
    bool isPanActive = false;

    public float keySpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        panSpeed = 1f;
        camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            camera.transform.position += new Vector3(0, keySpeed * Time.deltaTime,0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera.transform.position += new Vector3(0, -keySpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera.transform.position += new Vector3(keySpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            camera.transform.position += new Vector3(-keySpeed * Time.deltaTime, 0, 0);
        }


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
            camera.transform.position += (Vector3)mousePosDelta * panSpeed * Time.deltaTime;
        }

    }
}
