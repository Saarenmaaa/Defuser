using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public float damage = 10f; // Amount of damage dealt by the sword

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected"); // Debug log to confirm collision detection
        TargetScript target = collision.collider.GetComponent<TargetScript>();
        if (target != null)
        {
            Debug.Log("Applying Damage"); // Debug log to confirm damage application
            target.TakeDamage(damage);
        }
    }
}
