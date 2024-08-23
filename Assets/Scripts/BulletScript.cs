using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 1.0f;  // Set the damage value as needed

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that the bullet collided with is a target
        TargetScript target = collision.gameObject.GetComponent<TargetScript>();
        if (target != null)
        {
            target.TakeDamage(damage);

            // Optionally, destroy the bullet upon impact
            Destroy(gameObject);
        }
    }
}
