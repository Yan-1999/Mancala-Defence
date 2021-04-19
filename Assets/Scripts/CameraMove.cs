using UnityEngine;
using UnityEditor;

public class CameraMove : MonoBehaviour
{
    public bool canMove = false;
    float speed = 2.0f;

    float startTime = 2.0f;

    Vector3 startPosition;
    Vector3 endPosition;

    private void Update()
    {
        if (canMove)
        {
            float x = Mathf.Lerp(startPosition.x, endPosition.x, (Time.time - startTime) * speed);
            float y = Mathf.Lerp(startPosition.y, endPosition.y, (Time.time - startTime) * speed);
            float z = Mathf.Lerp(startPosition.z, endPosition.z, (Time.time - startTime) * speed);
            transform.position = new Vector3(x, y, z);
            if(transform.position==endPosition)
            {
                canMove = false;
            }
        }
    }

    public void MoveCamera(Vector3 start,Vector3 end)
    {
        startPosition = start;
        endPosition = end;
        startTime = Time.time;
        canMove = true;
    }
}