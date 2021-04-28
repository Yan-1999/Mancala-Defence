using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float KeyboardSpeed = 1;
    public float mouseSpeed = 300;
    public Vector3 originPosition;

    private void Start()
    {
        originPosition = transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = originPosition;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector3(h * KeyboardSpeed, -mouse * mouseSpeed, v * KeyboardSpeed) * Time.deltaTime, Space.World);
        GameManager.Instance.cameraPosition = transform.position;
    }
}
