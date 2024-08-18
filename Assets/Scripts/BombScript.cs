using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class BombScript : MonoBehaviour
{
    public float explosionDelay = 10f; // Time before the bomb explodes
    public float defuseTime = 5f; // Time required to defuse the bomb
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying the timer

    public Material activeMaterial1; // First material to indicate the bomb is active
    public Material activeMaterial2; // Second material to indicate the bomb is active (blinking)
    public Material defusingMaterial; // Material to indicate the bomb is being defused
    public Material defusedMaterial; // Material to indicate the bomb is defused

    [SerializeField] private Renderer targetRenderer; // Reference to the Renderer component of the cube

    private bool isDefusing = false;
    private bool isDefused = false;
    private float defuseStartTime;
    private bool playerInRange = false;
    private float defuseElapsedTime = 0f;
    private Coroutine blinkCoroutine;

    private void Start()
    {
        // Set the bomb to the initial active color and start blinking
        if (targetRenderer != null && activeMaterial1 != null)
        {
            SetMaterial(activeMaterial1);
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }
        // Start the countdown coroutine
        StartCoroutine(ExplodeBomb());
    }

    private IEnumerator ExplodeBomb()
    {
        float elapsedTime = 0f;

        while (elapsedTime < explosionDelay)
        {
            if (!isDefusing)
            {
                // Calculate remaining time
                float remainingTime = explosionDelay - elapsedTime;

                // Update the timer text
                if (timerText != null)
                {
                    timerText.text = Mathf.Ceil(remainingTime).ToString();
                }

                // Wait for a second before updating again
                yield return new WaitForSeconds(1f);

                // Increment elapsed time
                elapsedTime += 1f;
            }
            else
            {
                // If defusing is in progress, update timer to show remaining defuse time
                if (timerText != null)
                {
                    float defuseRemainingTime = Mathf.Max(0, defuseTime - defuseElapsedTime);
                    timerText.text = Mathf.Ceil(defuseRemainingTime).ToString();
                }

                if (defuseElapsedTime >= defuseTime)
                {
                    // Bomb is defused
                    isDefused = true;
                    if (targetRenderer != null && defusedMaterial != null)
                    {
                        SetMaterial(defusedMaterial);
                    }
                    Destroy(this.gameObject);
                    Debug.Log("Bomb defused!");
                    yield break;
                }

                // Wait for a short time to continue checking
                yield return null;
            }
        }

        // If the bomb was not defused and the time ran out
        if (!isDefused)
        {
            if (targetRenderer != null)
            {
                // Ensure it starts blinking if the bomb explodes
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                SetMaterial(activeMaterial1); // Set initial color before starting blink
                Debug.Log("Bomb exploded!");
            }
            timerText.text = "0";
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the tag "Player"
        {
            playerInRange = true;
            Debug.Log("Player entered range.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited range.");

            // Stop defusing if the player leaves range
            if (isDefusing)
            {
                isDefusing = false;
                Debug.Log("Defusing stopped.");

                // Reset the timer display to show remaining bomb time
                if (timerText != null)
                {
                    float remainingTime = explosionDelay - (Time.time - defuseStartTime);
                    timerText.text = Mathf.Ceil(remainingTime).ToString();
                }

                // Change material back to active if defusing stopped
                if (targetRenderer != null && activeMaterial1 != null)
                {
                    SetMaterial(activeMaterial1);
                }

                // Start blinking effect again
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
                blinkCoroutine = StartCoroutine(BlinkEffect());
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.F) && !isDefusing && !isDefused)
        {
            // Start defusing process if the key is held down
            if (!isDefusing)
            {
                isDefusing = true;
                defuseStartTime = Time.time;
                Debug.Log("Defusing started.");

                // Change material to indicate defusing state
                if (targetRenderer != null && defusingMaterial != null)
                {
                    SetMaterial(defusingMaterial);
                }

                // Stop blinking if defusing starts
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }
            }
        }
        else if (playerInRange && Input.GetKey(KeyCode.F) && isDefusing)
        {
            // Update defuse elapsed time
            defuseElapsedTime = Time.time - defuseStartTime;
        }
        else if (playerInRange && Input.GetKeyUp(KeyCode.F) && isDefusing)
        {
            // Cancel defusing if the key is released
            isDefusing = false;
            Debug.Log("Defusing stopped.");

            // Reset the timer display to show remaining bomb time
            if (timerText != null)
            {
                float remainingTime = explosionDelay - (Time.time - defuseStartTime);
                timerText.text = Mathf.Ceil(remainingTime).ToString();
            }

            // Change material back to active if defusing stopped
            if (targetRenderer != null && activeMaterial1 != null)
            {
                SetMaterial(activeMaterial1);
            }

            // Start blinking effect again
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }
    }

    private void SetMaterial(Material newMaterial)
    {
        // Apply the new material to the targetRenderer
        if (targetRenderer != null)
        {
            targetRenderer.material = newMaterial;
            Debug.Log($"Material changed to {newMaterial.name}");
        }
    }

    private IEnumerator BlinkEffect()
    {
        // Blink between two materials
        while (!isDefusing && !isDefused)
        {
            if (targetRenderer != null)
            {
                targetRenderer.material = activeMaterial1;
                yield return new WaitForSeconds(0.5f); // Adjust the time to change colors
                targetRenderer.material = activeMaterial2;
                yield return new WaitForSeconds(0.5f); // Adjust the time to change colors
            }
        }
    }
}
