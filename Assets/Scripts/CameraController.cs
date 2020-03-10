using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float cameraSpeed = 0.5f;
    public Transform target; // Drop the player in the inspector of the camera
    public float offset;

    private void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -10f);
    }

    void Update()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + offset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraSpeed * Time.deltaTime);
    }
}
