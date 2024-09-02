using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.5f;
    public Camera mainCamera;
    public float bombQuantity = 1;
    public Rigidbody rb;
    public float raycastDistance = 1;  // Distance to check for walls ahead
    public LayerMask wallLayer;  // Layer mask to specify the wall layer

    void Update()
    {
        // Get input from the user
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get mouse position on the screen and convert it to a point in the world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate the direction from the player to the mouse hit point
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 direction = targetPosition - transform.position;

            // Rotate the player to face the mouse cursor
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000f * Time.deltaTime);
            }
        }

        // Create a movement vector based on input in world space (ignoring player's rotation)
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Check if the Shift key is held down to enable sprinting
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movement *= sprintMultiplier;
        }

        // Raycast to detect walls or obstacles in the direction of movement, using the wall layer mask
        if (!IsObstacleAhead(movement))
        {
            // Apply movement to the player in world space
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private bool IsObstacleAhead(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance, wallLayer))
        {
            // If the raycast hits something on the wall layer, it means there's an obstacle ahead
            return true;
        }
        return false;
    }

    public void PlantBomb()
    {
        // Set the flag to indicate that a bomb has been planted
        bombQuantity--;
    }
}
