using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpecial : MonoBehaviour
{
    public GameObject bombPrefab; // The bomb prefab to instantiate
    public Transform player; // Reference to the player
    public int maxMines = 3; // The maximum number of mines a player can place
    private int currentMineCount = 0; // The current number of mines placed by the player

    void Update()
    {
        // Check if the player is in the bomb zone and the X key is pressed
        if (Input.GetButtonDown("Fire2"))
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        // Check if the player can place more mines
        if (CanPlaceMine())
        {
            Vector3 playerPosition = player.position + player.forward * 5f;
            playerPosition.y -= 0.5f;
            GameObject newMine = Instantiate(bombPrefab, playerPosition, Quaternion.identity);
            
            // Ensure the new mine has a MineScript component and assign a reference to this script
            MineScript mineScript = newMine.GetComponent<MineScript>();
            if (mineScript != null)
            {
                mineScript.SetTankSpecial(this); // Set the reference to TankSpecial
            }
            else
            {
                Debug.LogError("The bombPrefab does not have a MineScript component.");
            }

            // Update the mine count after placing the bomb
            PlaceMine();
            Debug.Log("Bomb placed. Current mine count: " + currentMineCount);
        }
        else
        {
            Debug.Log("Cannot place more mines. Maximum limit reached.");
        }
    }

    private bool CanPlaceMine()
    {
        return currentMineCount < maxMines;
    }

    private void PlaceMine()
    {
        if (CanPlaceMine())
        {
            currentMineCount++;
        }
    }

    public void MineExploded()
    {
        if (currentMineCount > 0)
        {
            currentMineCount--;
            Debug.Log("Mine exploded. Current mine count: " + currentMineCount);
        }
    }
}
