using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpecial : MonoBehaviour
{
    public GameObject bombPrefab; // The bomb prefab to instantiate
    public Transform player; // Reference to the player

    void Update()
    {
        // Check if the player is in the bomb zone and the X key is pressed
        if(Input.GetButtonDown("Fire2"))
        {
            PlaceBomb();
        }

    }

    private void PlaceBomb()
    {
        Vector3 playerPosition = player.position + player.forward * 5f;
        Instantiate(bombPrefab, playerPosition, Quaternion.identity);
    }
}

