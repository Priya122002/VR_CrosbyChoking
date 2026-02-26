using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class WireGenerator : MonoBehaviour
{
    [Header("Size")]
    public float totalLength = 6f;
    public float sag = 1.2f;

    [Header("Eye Shape")]
    public float eyeWidth = 0.4f;   // editable width
    public float eyeHeight = 0.7f;  // editable height

    [Header("Visual")]
    public float lineWidth = 0.08f;
    public Color ropeColor = new Color(0.7f, 0.7f, 0.7f);

    [Header("Quality")]
    [Range(10, 80)] public int mainSegments = 40;
    [Range(10, 80)] public int eyeSegments = 30;

    private LineRenderer lr;

    void OnEnable() => Build();
    void OnValidate() => Build();

    void Build()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        lr.useWorldSpace = false;
        lr.widthMultiplier = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = ropeColor;
        lr.endColor = ropeColor;

        float halfLength = totalLength * 0.5f;

        int totalPoints = (eyeSegments + 1) + mainSegments + (eyeSegments + 1);
        lr.positionCount = totalPoints;

        int index = 0;

        Vector3 leftCenter = new Vector3(-halfLength, 0, 0);
        Vector3 rightCenter = new Vector3(halfLength, 0, 0);

        // ================= LEFT OVAL =================
        for (int i = 0; i <= eyeSegments; i++)
        {
            float angle = i * Mathf.PI * 2f / eyeSegments;

            float x = Mathf.Cos(angle) * eyeWidth;
            float y = Mathf.Sin(angle) * eyeHeight;

            lr.SetPosition(index++, leftCenter + new Vector3(x, y, 0));
        }

        // Bottom connection points
        Vector3 leftBottom = leftCenter + new Vector3(0, -eyeHeight, 0);
        Vector3 rightBottom = rightCenter + new Vector3(0, -eyeHeight, 0);

        // ================= MAIN SAG =================
        for (int i = 0; i < mainSegments; i++)
        {
            float t = i / (float)(mainSegments - 1);

            float x = Mathf.Lerp(leftBottom.x, rightBottom.x, t);

            float y = Mathf.Lerp(leftBottom.y, rightBottom.y, t)
                      - 4f * sag * t * (1 - t);

            lr.SetPosition(index++, new Vector3(x, y, 0));
        }

        // ================= RIGHT OVAL =================
        for (int i = 0; i <= eyeSegments; i++)
        {
            float angle = i * Mathf.PI * 2f / eyeSegments;

            float x = Mathf.Cos(angle) * eyeWidth;
            float y = Mathf.Sin(angle) * eyeHeight;

            lr.SetPosition(index++, rightCenter + new Vector3(x, y, 0));
        }
    }
}