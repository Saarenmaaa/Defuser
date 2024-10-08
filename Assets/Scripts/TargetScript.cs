using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetScript : MonoBehaviour
{
    public float health = 5000f;
    public TextMeshProUGUI healthText;

    // Update is called once per frame
    void Update()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
