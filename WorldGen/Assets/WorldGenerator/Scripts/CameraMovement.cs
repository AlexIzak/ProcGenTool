using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    GameObject cam;

    [SerializeField]
    int camSpeed = 1;
    
    // Update is called once per frame
    void Update()
    {
        float x = cam.transform.position.x;
        float y = cam.transform.position.y;

        x += Input.GetAxis("Horizontal") * camSpeed;
        y += Input.GetAxis("Vertical") * camSpeed;

        cam.transform.position = new Vector3(x, y, -1);

        if(Input.GetKey(KeyCode.KeypadPlus)) cam.GetComponent<Camera>().orthographicSize += 1;
        if (Input.GetKey(KeyCode.KeypadMinus)) cam.GetComponent<Camera>().orthographicSize -= 1;
    }
}
