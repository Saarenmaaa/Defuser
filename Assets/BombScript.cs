using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class BombScript : MonoBehaviour
{
    public float explosionDelay = 10f; // Time before the bomb explodes
    public float defuseTime = 5f; // Time required to defuse the bomb
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying the timer

    private bool isDefusing = false;
    private bool isDefused = false;
    private float defuseStartTime;
    private bool playerInRange = false;

    private void Start()
    {
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
                    timerText.text = Mathf.Ceil(defuseTime - (Time.time - defuseStartTime)).ToString();
                }

                if (Time.time - defuseStartTime >= defuseTime)
                {
                    // Bomb is defused
                    isDefused = true;
                    Destroy(this.gameObject);
                    yield break;
                }

                // Wait for a short time to continue checking
                yield return null;
            }
        }

        // If the bomb was not defused and the time ran out
        if (!isDefused)
        {
            timerText.text = "0";
            Destroy(gameObject);
            Debug.Log("Bomb exploded!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the tag "Player"
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isDefusing && !isDefused)
        {
            // Start defusing process
            isDefusing = true;
            defuseStartTime = Time.time;
        }
    }
}
