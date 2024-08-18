using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.5f;
    public Camera mainCamera; // Reference to the main camera
    public float bombQuantity = 1;

    public Rigidbody rb;

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
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Normalize the movement vector to prevent faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Check if the Shift key is held down to enable sprinting
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movement *= sprintMultiplier;
        }

        // Apply movement to the player in world space
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Check for movement input (WASD, arrow keys, etc.)
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void PlantBomb()
    {
        // Set the flag to indicate that a bomb has been planted
        bombQuantity--;
    }
}

