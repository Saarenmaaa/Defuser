using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public float damage = 1.0f;
    public float lifetime = 2f;
    void Start()
    {
        // Destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }
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
