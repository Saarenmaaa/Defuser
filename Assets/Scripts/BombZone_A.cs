using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombZone_A : MonoBehaviour
{
    public GameObject bombPrefab; // The bomb prefab to instantiate
    public Transform player; // Reference to the player
    public float holdTime = 5f; // Time required to hold the key
    private bool isInBombZone = false; // To check if the player is in the bomb zone
    private float holdTimer = 0f; // Timer to track the key holding time
    private GameObject currentBomb; // Reference to the currently planted bomb

    private PlayerMovement PlayerScript; // Reference to the PlayerController script

    void Start()
    {
        // Get the PlayerController component from the player
        PlayerScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if the player is in the bomb zone and the X key is pressed
        if (isInBombZone && Input.GetKey(KeyCode.X))
        {
            holdTimer += Time.deltaTime;

            // Check if the key has been held for the required time
            if (holdTimer >= holdTime)
            {
                PlaceBomb();
                holdTimer = 0f; // Reset the timer
            }
        }
        else
        {
            holdTimer = 0f; // Reset the timer if the key is released or player leaves the zone
        }
    }

    private void PlaceBomb()
    {
        // Check if the player has already planted a bomb
        if (PlayerScript.bombQuantity <= 0)
        {
            Debug.Log("Player has already planted a bomb.");
            return;
        }

        // Check if a bomb has already been placed in this zone
        if (currentBomb != null)
        {
            Debug.Log("A bomb is already placed in this zone.");
            return;
        }

        // Instantiate the bomb at a position in front of the player
        Vector3 playerPosition = player.position + player.forward * 3f; // Adjust the distance as needed
        currentBomb = Instantiate(bombPrefab, playerPosition, Quaternion.identity);

        // Set the player’s bomb status
        PlayerScript.PlantBomb();

        // Subscribe to the bomb's explosion event
        BombScript bombScript = currentBomb.GetComponent<BombScript>();
        if (bombScript != null)
        {
            bombScript.OnBombExploded += HandleBombExplosion;
        }
    }

    private void HandleBombExplosion()
    {
        // Handle bomb explosion
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the bomb zone
        if (other.transform == player)
        {
            isInBombZone = true;
            Debug.Log("Player entered bombzone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the bomb zone
        if (other.transform == player)
        {
            isInBombZone = false;
            Debug.Log("Player exited bombzone");
        }
    }
}
