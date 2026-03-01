using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProtractorIndicator : MonoBehaviour
{
    [Header("UI References")]
    public Slider angleSlider;
    public RectTransform needle;
    public TextMeshProUGUI angleText;
    public Image sliderFill;

    [Header("Answer Panel")]
    public GameObject angleAnswerPanel;
    public Image answerImage;
    public TextMeshProUGUI answerText;

    [Header("Angle Settings")]
    public float minAngle = 0f;
    public float maxAngle = 180f;
    public float baseOffset = 90f;
    public float initialAngle = 10f;
    [Header("Answer Sprites")]
    public Sprite redSprite;
    public Sprite yellowSprite;
    public Sprite greenSprite;
    [Header("Zone Colors")]
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;

    private float lastChangeTime;
    private float stopDelay = 0.15f;
    private bool isDragging = false;

    void Start()
    {
        if (angleSlider != null)
        {
            angleSlider.minValue = minAngle;
            angleSlider.maxValue = maxAngle;
            angleSlider.value = initialAngle;

            angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateVisual(initialAngle);
        }

        if (angleAnswerPanel != null)
            angleAnswerPanel.SetActive(false);
    }

    void Update()
    {
        if (isDragging && Time.time - lastChangeTime > stopDelay)
        {
            isDragging = false;
            ApplyZoneFeedback(angleSlider.value);
        }
    }

    void OnSliderValueChanged(float value)
    {
        isDragging = true;
        lastChangeTime = Time.time;

        if (angleAnswerPanel != null)
            angleAnswerPanel.SetActive(false);

        UpdateVisual(value);
    }

    void UpdateVisual(float value)
    {
        float clamped = Mathf.Clamp(value, minAngle, maxAngle);

        // Rotate needle
        if (needle != null)
        {
            float finalRotation = baseOffset - clamped;
            needle.localRotation = Quaternion.Euler(0f, 0f, finalRotation);
        }

        // Update angle text
        if (angleText != null)
            angleText.text = Mathf.RoundToInt(clamped) + "°";
    }

    void ApplyZoneFeedback(float value)
    {
        float clamped = Mathf.Clamp(value, minAngle, maxAngle);

        Color zoneColor;
        string feedbackMessage;
        Sprite feedbackSprite;

        if (clamped <= 50f)
        {
            zoneColor = redColor;
            feedbackMessage = "Unsafe Angle";
            feedbackSprite = redSprite;
        }
        else if (clamped < 120f)
        {
            zoneColor = yellowColor;
            feedbackMessage = "Adjust Angle";
            feedbackSprite = yellowSprite;
        }
        else
        {
            zoneColor = greenColor;
            feedbackMessage = "Safe Angle";
            feedbackSprite = greenSprite;
        }

        // Apply slider color
        if (sliderFill != null)
            sliderFill.color = zoneColor;

        if (angleText != null)
            angleText.color = zoneColor;

        // Show answer panel
        if (angleAnswerPanel != null)
            angleAnswerPanel.SetActive(true);

        // Change sprite
        if (answerImage != null)
        {
            answerImage.sprite = feedbackSprite;
            answerImage.color = zoneColor;
        }

        if (answerText != null)
        {
            answerText.text = feedbackMessage;
            answerText.color = zoneColor;
        }
    }
}