using UnityEngine;

public class OrbitController : MonoBehaviour
{
    public Transform star;             // The star to orbit around (center point)
    public float semiMajorAxisAU;      // Orbital distance in AU (from JSON)
    public float orbitalPeriodDays;    // Orbital period in days (from JSON)
    public static float speedMultiplier = 10000f; // Global speed modifier (default 1)

    private float orbitalRadius;       // Converted to Unity units
    private float angularSpeed;        // Degrees per second

    // Scale factor to shrink AU into Unity space (tweakable)
    private const float AU_TO_UNITS = 50f;  // 1 AU = 50 Unity units (example)
    private const float VISUAL_ORBIT_MULTIPLIER = 140f; // makes orbits readable

    private Vector3 orbitAxis = Vector3.up; // rotation axis

    // Orbit ring parameters
    private const int orbitSegments = 100;
    private const float orbitLineWidth = 0.03f;
    private readonly Color orbitLineColor = Color.gray;

    private LineRenderer orbitLineRenderer;

    void Start()
    {
        if (star == null) return;

        // Convert orbital radius from AU to Unity units, apply visual multiplier
        orbitalRadius = semiMajorAxisAU * AU_TO_UNITS * VISUAL_ORBIT_MULTIPLIER;

        // Convert orbital period (days → seconds)
        float periodSeconds = orbitalPeriodDays * 86400f;

        // Angular speed in degrees per second
        angularSpeed = 360f / periodSeconds;

        // Place planet at initial position along +X axis relative to star (world space)
        transform.position = star.position + new Vector3(orbitalRadius, 0f, 0f);

        // Setup orbit ring LineRenderer
        GameObject orbitLineObject = new GameObject(gameObject.name + "_OrbitRing");
        orbitLineObject.transform.position = star.position;
        orbitLineObject.transform.rotation = Quaternion.identity;

        orbitLineRenderer = orbitLineObject.AddComponent<LineRenderer>();
        orbitLineRenderer.useWorldSpace = true;
        orbitLineRenderer.loop = true;
        orbitLineRenderer.positionCount = orbitSegments;

        // Make it white and slightly thicker for better visibility
        orbitLineRenderer.widthMultiplier = orbitLineWidth * 1.5f;
        Material orbitMat = new Material(Shader.Find("Unlit/Color"));
        Color orbitColor = Color.white * 0.8f;
        orbitMat.SetColor("_Color", orbitColor);

        // Enable faint glow (optional)
        orbitMat.EnableKeyword("_EMISSION");
        orbitMat.SetColor("_EmissionColor", orbitColor * 0.2f);

        orbitLineRenderer.material = orbitMat;
        orbitLineRenderer.startColor = orbitColor;
        orbitLineRenderer.endColor = orbitColor;

        // Calculate points for the orbit circle
        Vector3[] points = new Vector3[orbitSegments];
        float angleStep = 360f / orbitSegments;
        for (int i = 0; i < orbitSegments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = star.position.x + Mathf.Cos(angle) * orbitalRadius;
            float z = star.position.z + Mathf.Sin(angle) * orbitalRadius;
            points[i] = new Vector3(x, star.position.y, z);
        }
        orbitLineRenderer.SetPositions(points);
    }

    void Update()
    {
        if (star == null) return;

        // Orbit around star in world space
        transform.RotateAround(
            star.position,
            orbitAxis,
            angularSpeed * speedMultiplier * Time.deltaTime
        );
    }
}