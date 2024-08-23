using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TargetingSystem : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetMarkerPrefab;
    public GameObject missilePrefab;
    public LayerMask groundLayer;
    public float missileCooldown = 0.5f;  // Cooldown time between missile launches

    private List<GameObject> activeMissiles = new List<GameObject>();   // List to manage active missiles
    private Dictionary<GameObject, GameObject> missileMarkerMap = new Dictionary<GameObject, GameObject>(); // Map missiles to markers
    private const int maxMissiles = 3;
    private bool canLaunchMissile = true; // To manage cooldown

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canLaunchMissile && activeMissiles.Count < maxMissiles)
        {
            StartCoroutine(LaunchMissileAfterDelay());
        }
    }

    IEnumerator LaunchMissileAfterDelay()
    {
        canLaunchMissile = false;
        PlaceMarkerAndLaunchMissile();
        yield return new WaitForSeconds(missileCooldown);
        canLaunchMissile = true;
    }

    void PlaceMarkerAndLaunchMissile()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += 0.1f;  // Adjust the height of the marker

            // Create a new target marker at the clicked position
            GameObject targetMarker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);

            LaunchMissile(targetMarker);
        }
    }

    void LaunchMissile(GameObject targetMarker)
    {
        if (missilePrefab != null)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            HomingMissile missileScript = missile.GetComponent<HomingMissile>();

            if (missileScript != null)
            {
                missileScript.SetTarget(targetMarker.transform.position, targetMarker);
                missileScript.OnTargetHit += (marker) => HandleMissileHit(missile, marker);
            }

            activeMissiles.Add(missile); // Add the missile to the active missiles list
            missileMarkerMap[missile] = targetMarker; // Map this missile to its marker
        }
    }

    void HandleMissileHit(GameObject missile, GameObject marker)
    {
        // Remove the missile from the active missiles list
        activeMissiles.Remove(missile);

        // Remove the marker from the map and destroy it
        if (missileMarkerMap.TryGetValue(missile, out GameObject associatedMarker))
        {
            missileMarkerMap.Remove(missile);
            if (associatedMarker != null)
            {
                Destroy(associatedMarker);
            }
        }

        // Destroy the missile
        Destroy(missile);
    }
}
