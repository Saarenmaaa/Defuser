using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 5f;
    private Vector3 targetPosition;
    private bool targetSet = false;

    // Event to notify when the missile hits the target
    public delegate void TargetHit();
    public event TargetHit OnTargetHit;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        targetSet = true;
    }

    void Update()
    {
        if (targetSet)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                HitTarget();
            }
        }
    }

    void HitTarget()
    {
        if (OnTargetHit != null)
        {
            OnTargetHit();
        }
        Destroy(gameObject);  // Destroy the missile
    }
}
