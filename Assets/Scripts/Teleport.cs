using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Camera mainCamera;          // Reference to the main camera
    public LayerMask groundLayer;      // Layer mask for the ground
    public float dashCooldown = 2f;    // Cooldown between dashes
    public float lockedYPosition = 0f; // The locked Y position of the player
    private bool canDash = true;

    void Start()
    {
        lockedYPosition = transform.position.y;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(TeleportDash());
        }
    }

    private IEnumerator TeleportDash()
    {
        canDash = false; // Prevent further dashing until cooldown

        Vector3 targetPosition = GetTargetPosition(); // Determine the target position

        if (targetPosition != Vector3.zero) // Ensure a valid target position
        {
            // Set the Y position to the locked Y position
            targetPosition.y = lockedYPosition;

            // Move the player to the target position
            transform.position = targetPosition;
        }

        yield return new WaitForSeconds(dashCooldown); // Wait for the cooldown period
        canDash = true; // Re-enable dashing
    }

    private Vector3 GetTargetPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast and check if it hits the ground layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Return the position on the ground where the raycast hits
            return hit.point;
        }

        return Vector3.zero; // Return zero vector if no valid position is found
    }
}