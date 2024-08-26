using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpecial : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetMarkerPrefab;
    public GameObject smokeGrenadePrefab;
    public LayerMask groundLayer;
    public float smokeCooldown = 3f;
    private Dictionary<GameObject, GameObject> grenadeMarkerMap = new Dictionary<GameObject, GameObject>();
    private bool canLaunchGrenade = true;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && canLaunchGrenade)
        {
            StartCoroutine(LaunchSmokeAfterDelay());
        }
    }

    IEnumerator LaunchSmokeAfterDelay()
    {
        canLaunchGrenade = false;
        PlaceMarkerAndLaunchSmoke();
        yield return new WaitForSeconds(smokeCooldown);
        canLaunchGrenade = true;
    }

    void PlaceMarkerAndLaunchSmoke()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += 0.1f;
            GameObject targetMarker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
            LaunchSmoke(targetMarker, targetPosition);
        }
    }

    void LaunchSmoke(GameObject targetMarker, Vector3 targetPosition)
    {
        GameObject smokeGrenade = Instantiate(smokeGrenadePrefab, transform.position, Quaternion.identity);
        
        // Set the initial rotation to face the target immediately
        Vector3 direction = targetPosition - smokeGrenade.transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            smokeGrenade.transform.rotation = targetRotation;
        }

        SmokeScript smokeScript = smokeGrenade.GetComponent<SmokeScript>();
        smokeScript.SetTarget(targetPosition, targetMarker);

        // Store the grenade and its associated marker in the dictionary
        grenadeMarkerMap[smokeGrenade] = targetMarker;
    }
}
