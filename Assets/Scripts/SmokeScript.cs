using UnityEngine;
using System;

public class SmokeScript : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 targetPosition;
    private GameObject targetMarker;

    public event Action<GameObject, GameObject> OnTargetHit;

    public void SetTarget(Vector3 position, GameObject marker)
    {
        targetPosition = position;
        targetMarker = marker;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            OnTargetHit?.Invoke(gameObject, targetMarker);
        }
    }
}
