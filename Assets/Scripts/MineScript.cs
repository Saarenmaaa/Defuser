using System.Collections;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public Material defaultMaterial; // The default material of the mine.
    public Material blinkMaterial; // The material used for the blinking effect.
    public GameObject blinkingPart; // Reference to the specific child GameObject that should blink.
    private Renderer blinkingRenderer; // The Renderer of the blinking part.
    private Coroutine blinkCoroutine; // Reference to the blinking coroutine.

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TargetScript targetScript = other.GetComponent<TargetScript>();
            if (targetScript != null)
            {
                targetScript.TakeDamage(50f);
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator BlinkEffect()
    {
        while (true)
        {
            blinkingRenderer.material = blinkMaterial;
            yield return new WaitForSeconds(0.5f);
            blinkingRenderer.material = defaultMaterial;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
