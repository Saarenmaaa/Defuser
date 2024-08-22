using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public float health = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the target
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that hit the target is a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);

                // Optionally, destroy the bullet upon impact
                Destroy(collision.gameObject);
            }
        }
    }
}
