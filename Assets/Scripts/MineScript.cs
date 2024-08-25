using System.Collections;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public Material defaultMaterial; // The default material of the mine.
    public Material blinkMaterial; // The material used for the blinking effect.
    public GameObject blinkingPart; // Reference to the specific child GameObject that should blink.
    public float explosionRadius = 5f; // The radius of the explosion effect.
    public float explosionDamage = 50f; // The amount of damage dealt in the explosion.
    private Renderer blinkingRenderer; // The Renderer of the blinking part.
    private Coroutine blinkCoroutine; // Reference to the blinking coroutine.
    private bool isActivated = false; // Track if the mine has been activated
    private TankSpecial tankSpecial; // Reference to the TankSpecial script

    private void Start()
    {
        // Get the Renderer component of the specified blinking part
        if (blinkingPart != null)
        {
            blinkingRenderer = blinkingPart.GetComponent<Renderer>();

            // Ensure the blinking part has a Renderer
            if (blinkingRenderer != null)
            {
                blinkingRenderer.material = defaultMaterial; // Set the default material.
            }
            else
            {
                Debug.LogWarning("No Renderer component found on the specified blinking part.");
            }
        }
        else
        {
            Debug.LogWarning("Blinking part not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the blinking effect when the player enters the trigger zone.
            if (blinkingRenderer != null)
            {
                blinkCoroutine = StartCoroutine(BlinkEffect());
            }

            // Start the explosion countdown.
            if (!isActivated)
            {
                isActivated = true;
                StartCoroutine(ExplosionCountdown());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the blinking effect
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
                if (blinkingRenderer != null)
                {
                    blinkingRenderer.material = defaultMaterial; // Reset to default material
                }
            }

            // If the mine is activated, explode immediately if the player exits the trigger zone.
            if (isActivated)
            {
                Explode();
            }
        }
    }

    private IEnumerator BlinkEffect()
    {
        while (true)
        {
            if (blinkingRenderer != null)
            {
                blinkingRenderer.material = blinkMaterial;
                yield return new WaitForSeconds(0.5f);
                blinkingRenderer.material = defaultMaterial;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield break;
            }
        }
    }

    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(3f);

        // If still activated, trigger the explosion
        if (isActivated)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Get all colliders within the explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Deal damage to each player within the explosion radius
                TargetScript targetScript = hitCollider.GetComponent<TargetScript>();
                if (targetScript != null)
                {
                    targetScript.TakeDamage(explosionDamage);
                }
            }
        }

        // Notify the TankSpecial that a mine has exploded
        if (tankSpecial != null)
        {
            tankSpecial.MineExploded();
        }
        else
        {
            Debug.LogError("TankSpecial reference is missing in MineScript.");
        }

        // Destroy the mine after explosion
        Destroy(gameObject);
    }

    // Method to set the reference to TankSpecial
    public void SetTankSpecial(TankSpecial tankSpecial)
    {
        this.tankSpecial = tankSpecial;
    }
}
