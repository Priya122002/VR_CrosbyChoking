
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class WebSlingSemiCircle : MonoBehaviour
{
    [Header("Size")]
    public float totalWidth = 1.6f;
    public float sag = 0.7f;

    [Header("Top Lift")]
    public float endLift = 0.6f;  // lifts ends slightly

    [Header("Visual")]
    public float strapWidth = 0.06f;
    public Color strapColor = new Color(0.5f, 0.1f, 0.6f);

    [Header("Quality")]
    [Range(50,200)] public int segments = 120;

    LineRenderer lr;

    void OnEnable() => Build();
    void OnValidate() => Build();

    void Build()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        lr.useWorldSpace = false;
        lr.loop = false;
        lr.widthMultiplier = strapWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = strapColor;
        lr.endColor = strapColor;

        lr.positionCount = segments;

        float half = totalWidth * 0.5f;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            float x = Mathf.Lerp(-half, half, t);

            // Smooth rounded gravity curve
            float y = -sag * Mathf.Sin(t * Mathf.PI);

            // Optional: very small end lift for realism
            float lift = Mathf.Pow(Mathf.Abs(t - 0.5f) * 2f, 1.5f) * endLift;
            y += lift;

            lr.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}