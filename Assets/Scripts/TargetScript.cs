using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public float health = 5f;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the target when health is depleted
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
