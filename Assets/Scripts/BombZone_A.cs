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
    private GameObject currentBomb; // Reference to the current bomb in the zone

    void Update()
    {
        // Check if the player is in the bomb zone and the C key is pressed
        if (isInBombZone && Input.GetKey(KeyCode.C))
        {
            holdTimer += Time.deltaTime;

            // Check if the key has been held for the required time
            if (holdTimer >= holdTime && currentBomb == null)
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
        // Instantiate the bomb at the player's position
        currentBomb = Instantiate(bombPrefab, player.position, Quaternion.identity);
        // Subscribe to the bomb's explosion event
        BombScript bombScript = currentBomb.GetComponent<BombScript>();
        if (bombScript != null)
        {
            bombScript.OnBombExploded += HandleBombExplosion;
        }
    }

    private void HandleBombExplosion()
    {
        // Destroy the bomb zone when the bomb explodes
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the bomb zone
        if (other.transform == player)
        {
            isInBombZone = true;
            Debug.Log("Player entered bomb zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the bomb zone
        if (other.transform == player)
        {
            isInBombZone = false;
            Debug.Log("Player exited bomb zone");
        }
    }
}
