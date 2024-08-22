using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifetime = 2f; // Time in seconds before the projectile is destroyed
    public float damage = 1;
    
    void Start()
    {
        // Destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }

}

