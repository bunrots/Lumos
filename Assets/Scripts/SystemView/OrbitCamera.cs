using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target & Distance")]
    public Transform target;
    public float distance = 100f;       // Starting distance
    public float minDistance = 20f;
    public float maxDistance = 1000f;

    [Header("Speed Settings")]
    public float orbitSpeed = 100f;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 50f;

    private Vector3 angles;
    private Vector3 lastMousePosition;

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
        transform.rotation = rotation;

        // Update position based on rotation and distance
        transform.position = target.position - transform.forward * distance;
    }
}