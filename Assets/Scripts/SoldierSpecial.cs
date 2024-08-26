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
            targetPosition.y += 0.1f;  // Adjust marker position slightly above the ground
            GameObject targetMarker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
            LaunchSmoke(targetMarker);
        }
    }

    void LaunchSmoke(GameObject targetMarker)
    {
        if (smokeGrenadePrefab != null)
        {
            GameObject smokeGrenade = Instantiate(smokeGrenadePrefab, transform.position, Quaternion.identity);
            SmokeScript smokeGrenadeScript = smokeGrenade.GetComponent<SmokeScript>();

            if (smokeGrenadeScript != null)
            {
                smokeGrenadeScript.SetTarget(targetMarker.transform.position, targetMarker);
                smokeGrenadeScript.OnTargetHit += HandleGrenadeHit;
            }

            grenadeMarkerMap[smokeGrenade] = targetMarker; // Map this grenade to its marker
        }
    }

    void HandleGrenadeHit(GameObject smokeGrenade, GameObject marker)
    {
        // Remove the marker from the map and destroy it
        if (grenadeMarkerMap.TryGetValue(smokeGrenade, out GameObject associatedMarker))
        {
            grenadeMarkerMap.Remove(smokeGrenade);
            if (associatedMarker != null)
            {
                Destroy(associatedMarker);
            }
        }

        // Destroy the grenade
        Destroy(smokeGrenade);
    }
}
