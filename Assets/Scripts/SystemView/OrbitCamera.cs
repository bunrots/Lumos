using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target & Distance")]
    public Transform target;
    public float distance = 200f;       // Starting distance
    public float minDistance = 20f;
    public float maxDistance = 1000f;

    [Header("Speed Settings")]
    public float orbitSpeed = 100f;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 50f;

    public bool useSmoothTransition = true;

    // New: focus target
    private Transform focusTarget = null;
    private bool isTransitioning = false;

    private Vector3 angles;
    private Vector3 lastMousePosition;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("OrbitCamera: No target assigned!");
            return;
        }

        // Set initial angles based on current rotation
        angles = transform.eulerAngles;

        // Position camera at starting distance
        transform.position = target.position - transform.forward * distance;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Handle orbit (right-click)
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            angles.x -= delta.y * orbitSpeed * Time.deltaTime;
            angles.y += delta.x * orbitSpeed * Time.deltaTime;
        }

        // Handle pan (middle-click)
        if (Mouse.current.middleButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            Vector3 pan = -transform.right * delta.x * panSpeed * Time.deltaTime;
            pan += -transform.up * delta.y * panSpeed * Time.deltaTime;
            transform.position += pan;
            target.position += pan;
        }

        // Handle zoom
        float scroll = Mouse.current.scroll.ReadValue().y;
        distance -= scroll * zoomSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Apply rotation
        Quaternion rotation = Quaternion.Euler(angles.x, angles.y, 0);

        if (isTransitioning && focusTarget != null)
        {
            targetRotation = rotation;
            targetPosition = focusTarget.position - targetRotation * Vector3.forward * distance;

            if (useSmoothTransition)
            {
                // Smoothly interpolate position and rotation
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

                // Check if close enough to stop transitioning
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f && Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    isTransitioning = false;
                }
            }
            else
            {
                // Immediately set position and rotation without smoothing
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isTransitioning = false;
            }
        }
        else if (focusTarget != null && !useSmoothTransition)
        {
            targetRotation = rotation;
            targetPosition = focusTarget.position - targetRotation * Vector3.forward * distance;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
        else
        {
            // Normal orbit around main target
            targetRotation = rotation;
            targetPosition = target.position - targetRotation * Vector3.forward * distance;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    /// <summary>
    /// Smoothly move the camera to focus on a new target, but do not change the actual orbit/pan target.
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="newDistance"></param>
    public void SetTargetSmooth(Transform newTarget, float newDistance = -1f)
    {
        // Do NOT overwrite the main orbit target.
        focusTarget = newTarget;
        isTransitioning = true;

        if (newDistance > 0f)
        {
            distance = Mathf.Clamp(newDistance, minDistance, maxDistance);
        }
    }

    public void ToggleSmooth()
    {
        useSmoothTransition = !useSmoothTransition;
        Debug.Log("Camera smooth transition is now: " + useSmoothTransition);
    }
}