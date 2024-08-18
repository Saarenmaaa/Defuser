using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;   // Reference to the projectile prefab
    public Transform firePoint;            // The point from which the projectile will be fired
    public float projectileSpeed = 20f;    // Speed at which the projectile moves

    public float clipSize = 10;            // Maximum ammo capacity
    private float currentAmmo = 0;         // Current ammo in the clip
    public float reloadDelay = 1.5f;       // Time taken to reload

    private bool isReloading = false;      // Is the gun currently reloading?

    void Start()
    {
        // Initialize current ammo
        currentAmmo = clipSize;
    }

    void Update()
    {
        // Check if the player presses the fire button (e.g., left mouse button)
        if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && !isReloading)
        {
            Shoot();
        }

        // Check if the player presses the reload button (e.g., "R")
        if (Input.GetKey(KeyCode.R) && currentAmmo < clipSize && !isReloading)
        {
            StartCoroutine(Reload());
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

        // Decrease the ammo count
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Optionally, you could play a reload animation or sound here

        // Wait for the reload delay
        yield return new WaitForSeconds(reloadDelay);

        // Refill the ammo
        currentAmmo = clipSize;

        isReloading = false;
        Debug.Log("Reload complete.");
    }
}
