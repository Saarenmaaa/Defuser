using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 2f;
    public int directHitDamage = 50;  // Damage for a direct hit
    public int maxSplashDamage = 50;  // Maximum splash damage
    public int minSplashDamage = 1;   // Minimum splash damage
    public float splashRadius = 5f;   // Radius within which to apply splash damage
    public float explosionTriggerDistance = 1f; // Distance within which explosion should be triggered
    public float minimumDamageRange = 0.5f; // Minimum distance for full damage

    private Vector3 targetPosition;
    private GameObject targetMarker;     // Reference to the target marker

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
            // Trigger explosion if close to the target marker
            HandleExplosion();
            Destroy(gameObject); // Destroy the missile after handling explosion
        }
    }


    private void HandleExplosion()
    {
        if (targetMarker != null)
        {
            // Perform an overlap sphere check to find objects within the splash radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRadius);

            foreach (var hitCollider in hitColliders)
            {
                TargetScript target = hitCollider.GetComponent<TargetScript>();
                if (target != null)
                {
                    // Calculate damage based on the distance between the marker and the target
                    float distanceToMarker = Vector3.Distance(targetMarker.transform.position, target.transform.position);
                    int damage = Mathf.RoundToInt(CalculateSplashDamage(distanceToMarker));
                    ApplyDamageToTarget(damage, target);
                }
            }

            // Notify listeners that the missile exploded
            OnTargetHit?.Invoke(targetMarker);

            // Destroy the marker after the explosion has been handled
            Destroy(targetMarker);
        }
    }

    private float CalculateSplashDamage(float distance)
    {
        if (distance <= splashRadius)
        {
            // Calculate damage with minimum damage range consideration
            if (distance <= minimumDamageRange)
            {
                return maxSplashDamage; // Full damage if within minimum range
            }
            
            // Calculate damage using a quadratic falloff for smoother transition
            float normalizedDistance = distance / splashRadius; // Normalize distance to a value between 0 and 1
            float damage = Mathf.Lerp(maxSplashDamage, minSplashDamage, normalizedDistance * normalizedDistance);
            return damage;
        }
        return 0f;
    }

    private void ApplyDamageToTarget(int damage, TargetScript target)
    {
        if (target != null)
        {
            Debug.Log($"Explosion damage {target.gameObject.name}, {damage}");
            target.TakeDamage(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TargetScript target = collision.gameObject.GetComponent<TargetScript>();

        if (target != null)
        {
            // Apply direct hit damage to the target
            Debug.Log($"Missile directly hit target: {collision.gameObject.name} takes {directHitDamage} damage.");
            target.TakeDamage(directHitDamage);
        }

        // Destroy the missile upon impact
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        HandleExplosion();
    }
}
