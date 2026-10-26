using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class WaveWall : MonoBehaviour
{
    [Header("Wave Shape")]
    public float height = 15f;
    public float amplitude = 0.2f;
    public float frequency = 10f;

    [Range(2, 200)]
    public int segments = 200;

    public bool flipWave = false;

    [Header("Wall Appearance")]
    public float wallWidth = 0.12f;
    public Material wallMaterial;
    public float colliderOffset = 0f;

    private EdgeCollider2D edge;
    private LineRenderer line;

    void OnEnable()
    {
        Initialize();
        UpdateWall();
    }

    void OnValidate()
    {
        Initialize();
        UpdateWall();
    }

    void Update()
    {
        // Cho phép chỉnh realtime trong Play Mode.
        //if (Application.isPlaying)
        //    UpdateWall();
    }

    void Initialize()
    {
        if (edge == null)
            edge = GetComponent<EdgeCollider2D>();

        if (line == null)
            line = GetComponent<LineRenderer>();

        if (line != null)
        {
            line.useWorldSpace = false;
            line.loop = false;
            line.widthMultiplier = 1f;

            if (wallMaterial != null)
                line.sharedMaterial = wallMaterial;
        }
    }

    void UpdateWall()
    {
        if (edge == null || line == null)
            return;

        int count = Mathf.Max(2, segments);

        Vector2[] points = new Vector2[count + 1];
        Vector3[] positions = new Vector3[count + 1];

        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            float y = -height / 2f + t * height;

            float x = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude;

            if (flipWave)
                x = -x;

            positions[i] = new Vector3(x, y, 0f);
            points[i] = new Vector2(x + colliderOffset, y);
        }

        edge.points = points;

        line.positionCount = positions.Length;
        line.SetPositions(positions);

        line.startWidth = wallWidth;
        line.endWidth = wallWidth;

        if (wallMaterial != null)
            line.sharedMaterial = wallMaterial;
    }
}