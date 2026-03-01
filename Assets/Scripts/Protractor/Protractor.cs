using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Protractor : MonoBehaviour
{
    public RectTransform numbersParent;
    public GameObject textPrefab;

    [Header("Protractor Settings")]
    public float radius = 300f;
    public int angleStep = 10;
    public float startAngle = 0f;
    public float endAngle = 180f;

    private void OnEnable()
    {
        GenerateNumbers();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        GenerateNumbers();
    }
#endif

    public void GenerateNumbers()
    {
        if (numbersParent == null || textPrefab == null)
            return;

        // Clear old numbers safely
        for (int i = numbersParent.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(numbersParent.GetChild(i).gameObject);
            else
                DestroyImmediate(numbersParent.GetChild(i).gameObject);
        }

        for (int angle = (int)startAngle; angle <= endAngle; angle += angleStep)
        {
            GameObject obj = Instantiate(textPrefab, numbersParent);
            obj.name = "Angle_" + angle;

            RectTransform rect = obj.GetComponent<RectTransform>();
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

            if (text == null)
            {
                Debug.LogError("TextMeshProUGUI missing on prefab!");
                return;
            }

            text.text = angle + "°";

            // Center anchors
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            float rad = angle * Mathf.Deg2Rad;

            float x = -Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            rect.anchoredPosition = new Vector2(x, y);

            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;   // keep clean scale
        }
    }
}