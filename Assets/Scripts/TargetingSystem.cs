using UnityEngine;
using System.Collections.Generic;

public class TargetingSystem : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject targetMarkerPrefab;
    public GameObject missilePrefab;        // Prefab for the missile
    public LayerMask groundLayer;

    private List<GameObject> activeMissiles = new List<GameObject>();   // List to manage active missiles
    private List<GameObject> activeMarkers = new List<GameObject>();    // List to manage active markers
    private const int maxMissiles = 3;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && activeMissiles.Count < maxMissiles)
        {
            PlaceMarkerAndLaunchMissile();
        }
    }

    void PlaceMarkerAndLaunchMissile()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            // Create a new target marker at the clicked position
            GameObject targetMarker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
            activeMarkers.Add(targetMarker);  // Add the marker to the active markers list

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
                missileScript.SetTarget(targetMarker.transform.position);
                missileScript.OnTargetHit += () => HandleMissileHit(missile, targetMarker);  // Pass both missile and marker
            }

            activeMissiles.Add(missile);  // Add the missile to the active missiles list
        }
    }

    void HandleMissileHit(GameObject missile, GameObject targetMarker)
    {
        // Remove the missile and marker from their respective lists
        activeMissiles.Remove(missile);
        activeMarkers.Remove(targetMarker);

        // Destroy the missile and marker
        Destroy(missile);
        Destroy(targetMarker);
    }
}
