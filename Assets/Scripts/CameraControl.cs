using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float KeyboardSpeed = 1;
    public float mouseSpeed = 300;
    public Vector3 originPosition;
    public float left;
    public float right;
    public float up;
    public float down;
    public float low;
    public float high;
    private void Start()
    {
        originPosition = transform.position;
    }

    void Update()
    {
        if (!PlayerInterface.Instance.IsUpgrading)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = originPosition;
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float mouse = Input.GetAxis("Mouse ScrollWheel");
            transform.Translate(new Vector3(h * KeyboardSpeed, -mouse * mouseSpeed, v * KeyboardSpeed) * Time.deltaTime, Space.World);
            CheckPosition();
            GameManager.Instance.cameraPosition = transform.position;
        }
    }
    private void CheckPosition()
    {
        float x = transform.position.x, y = transform.position.y, z = transform.position.z;
        if (x < left)
        {
            x = left;
        }
        else if (x > right)
        {
            x = right;
        }
        if (y > high)
        {
            y = high;
        }
        else if (y < low)
        {
            y = low;
        }
        if (z < down)
        {
            z = down;
        }
        else if (z > up)
        {
            z = up;
        }
        transform.position = new Vector3(x, y, z);
    }
}
