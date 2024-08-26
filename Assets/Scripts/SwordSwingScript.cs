using System.Collections;
using UnityEngine;

public class SwordSwingScript : MonoBehaviour
{
    public Animator animator;
    public float swingDuration = 0.1f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Trigger the sword swing on input
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        animator.Play("SwordSwing"); // Play the swing animation
        yield return new WaitForSeconds(swingDuration); // Wait for the swing duration
    }
}
