using TMPro;
using UnityEngine;

public class Protractor : MonoBehaviour
{
    public RectTransform numbersParent;
    public GameObject textPrefab;

    [Header("Protractor Settings")]
    public float radius = 300f;
    public int angleStep = 10;
    public float startAngle = 0f;
    public float endAngle = 180f;

    private bool generated = false;

    private void Start()
    {
        if (!generated)
        {
            GenerateNumbers();
            generated = true;
        }
    }

    public void GenerateNumbers()
    {
        if (numbersParent == null || textPrefab == null)
            return;

        if (numbersParent.childCount > 0)
            return;   // 🔒 Prevent regenerating

        for (int angle = (int)startAngle; angle <= endAngle; angle += angleStep)
        {
            GameObject obj = Instantiate(textPrefab, numbersParent);
            obj.name = "Angle_" + angle;

            RectTransform rect = obj.GetComponent<RectTransform>();
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

            text.text = angle + "°";

            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            float rad = angle * Mathf.Deg2Rad;
            float x = -Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            rect.anchoredPosition = new Vector2(x, y);
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;
        }
    }
}