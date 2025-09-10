using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class OrbitTrailController : MonoBehaviour
{
    [Header("Trail Appearance")]
    public float trailWidth = 0.3f;
    public Color trailColor = Color.white;
    [Range(0.01f, 1f)]
    public float visibleFractionOfOrbit = 0.3f;
    public AnimationCurve widthByDistance = AnimationCurve.Linear(0f,1f,100f,1f);

    [Header("Glow Settings")]
    public bool enableGlow = true;
    [Range(0f, 1f)]
    public float glowIntensity = 0.3f;

    private TrailRenderer trail;
    private OrbitController orbit;
    private Transform star;

    void Awake()
    {
        orbit = GetComponent<OrbitController>();
        trail = GetComponent<TrailRenderer>();
        if (trail == null) trail = gameObject.AddComponent<TrailRenderer>();
        if (orbit != null) star = orbit.star;

        ConfigureTrailRenderer();
    }

    void ConfigureTrailRenderer()
    {
        trail.time = 1f;
        trail.startWidth = trailWidth;
        trail.endWidth = trailWidth;
        trail.numCornerVertices = 6;
        trail.numCapVertices = 6;
        trail.autodestruct = false;
        trail.alignment = LineAlignment.View;

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        Material mat = (shader != null) ? new Material(shader) : new Material(Shader.Find("Unlit/Color"));
        mat.SetColor("_BaseColor", trailColor);

        if (enableGlow && mat.HasProperty("_EmissionColor"))
        {
            Color emissionColor = trailColor.linear * glowIntensity;
            mat.SetColor("_EmissionColor", emissionColor);
            mat.EnableKeyword("_EMISSION");
        }

        trail.material = mat;
        trail.enabled = true;
    }

    void Start()
    {
        UpdateTrailLifetime();
    }

    void Update()
    {
        UpdateTrailLifetime();

        if (trail != null)
        {
            trail.startWidth = trailWidth;
            trail.endWidth = trailWidth;

            if (enableGlow && trail.material.HasProperty("_EmissionColor"))
            {
                Color emissionColor = trailColor.linear * glowIntensity;
                trail.material.SetColor("_EmissionColor", emissionColor);
            }
        }
    }

    void UpdateTrailLifetime()
    {
        if (orbit == null || orbit.orbitalPeriodDays <= 0f) return;

        float periodSeconds = orbit.orbitalPeriodDays * 86400f;
        float globalSpeed = OrbitController.speedMultiplier;
        if (globalSpeed <= 0f) globalSpeed = 1f;

        float lifetime = (periodSeconds / globalSpeed) * Mathf.Clamp01(visibleFractionOfOrbit);
        lifetime = Mathf.Max(0.1f, Mathf.Min(lifetime, 3000f));
        trail.time = lifetime;
    }
}