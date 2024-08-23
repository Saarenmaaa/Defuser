using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 2f;
    public float damage = 1f;  // Damage the missile does
    private Vector3 targetPosition;
    private GameObject targetMarker;  // Reference to the target marker
    public delegate void TargetHitDelegate(GameObject marker);
    public event TargetHitDelegate OnTargetHit;

    public void SetTarget(Vector3 target, GameObject marker)
    {
        targetPosition = target;
        targetMarker = marker;  // Set the marker associated with this missile
    }

    void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            // Notify listeners that the target was hit
            OnTargetHit?.Invoke(targetMarker); // Pass the target marker
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Notify listeners that the missile hit something
        TargetScript target = collision.gameObject.GetComponent<TargetScript>();
        if (target != null)
        {
            target.TakeDamage(damage); // Apply damage to the target
            OnTargetHit?.Invoke(collision.gameObject); // Pass the target marker
        }
        else
        {
            OnTargetHit?.Invoke(targetMarker); // Pass the marker or null
        }

        // Destroy the missile upon impact
        Destroy(gameObject);
    }
}
