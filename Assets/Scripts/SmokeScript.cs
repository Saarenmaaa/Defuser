using UnityEngine;

public class SmokeScript : MonoBehaviour
{
    public float speed = 20f;
    public float rotationSpeed = 20f;
    private Vector3 targetPosition;
    private GameObject targetMarker;

    public void SetTarget(Vector3 target, GameObject marker)
    {
        targetPosition = target;
        targetMarker = marker;
    }

    void Update()
    {
        Vector3 direction = targetPosition - transform.position;

        if (direction.magnitude > 0.1f)
        {
            // Rotate smoothly towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move forward in the direction the grenade is facing
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            Destroy(targetMarker);
            Destroy(gameObject);
        }
    }
}
