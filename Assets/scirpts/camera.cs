using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Smooth Follow Settings")]
    public float smoothTime = 0.2f;           // Lower = snappier, try 0.1 to 0.3
    public Vector2 offset = Vector2.zero;    // Camera offset
    private Vector3 velocity = Vector3.zero; // Internal for SmoothDamp

    [Header("Clamp Camera Movement (Optional)")]
    public bool enableBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

            // SmoothDamp for responsive smooth following
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            // Clamp within bounds if enabled
            if (enableBounds)
            {
                float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
                float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
                transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);
            }
            else
            {
                transform.position = smoothedPosition;
            }
        }
    }
}
