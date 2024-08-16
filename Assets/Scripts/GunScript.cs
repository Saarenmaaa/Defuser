using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;   // Reference to the projectile prefab
    public Transform firePoint;            // The point from which the projectile will be fired
    public float projectileSpeed = 20f;    // Speed at which the projectile moves

    void Update()
    {
        // Check if the player presses the fire button (e.g., left mouse button)
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile at the fire point
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody component and set its velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }
    }
}
