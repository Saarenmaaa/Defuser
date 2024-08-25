using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // The target object to follow
    public Vector3 offset;            // The offset from the target position
    public Vector3 rotation = new Vector3(45f, 15.5f, -18f);  // The desired rotation

    void Start()
    {
        // Set the camera's rotation to the specified values
        transform.rotation = Quaternion.Euler(rotation);
    }

    void LateUpdate()
    {
        // Check if the target is not null before trying to follow it
        if (target != null)
        {
            // Follow the target with the specified offset
            transform.position = target.position + offset;
        }
        else
        {
            Debug.LogWarning("Target has been destroyed. Camera cannot follow.");
        }
    }
}
