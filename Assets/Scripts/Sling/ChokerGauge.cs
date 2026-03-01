using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChokerGauge : MonoBehaviour
{
    [Header("UI References")]
    public Image greenFill;
    public Image yellowFill;
    public Image redFill;
    public TextMeshProUGUI percentageText;

    [Header("Targets (Set in Inspector)")]
    [Range(0f, 1f)] public float greenTarget = 0.75f;
    [Range(0f, 1f)] public float yellowTarget = 0.85f;
    [Range(0f, 1f)] public float redTarget = 1f;

    [Header("Animation Speed")]
    public float animationSpeed = 0.3f;

    private float currentGreen = 0f;
    private float currentYellow = 0f;
    private float currentRed = 0f;

    private Coroutine animationRoutine;

    private void Start()
    {
        greenFill.fillAmount = 0f;
        yellowFill.fillAmount = 0f;
        redFill.fillAmount = 0f;
        percentageText.text = "0%";
    }

    // 🔥 Call this only
    public void UpdateRope()
    {
        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(AnimateGauge());
    }

    IEnumerator AnimateGauge()
    {
        while (currentGreen < greenTarget ||
               currentYellow < yellowTarget ||
               currentRed < redTarget)
        {
            currentGreen = Mathf.MoveTowards(currentGreen, greenTarget, Time.deltaTime * animationSpeed);
            currentYellow = Mathf.MoveTowards(currentYellow, yellowTarget, Time.deltaTime * animationSpeed);
            currentRed = Mathf.MoveTowards(currentRed, redTarget, Time.deltaTime * animationSpeed);

            greenFill.fillAmount = currentGreen;
            yellowFill.fillAmount = currentYellow;
            redFill.fillAmount = currentRed;

            percentageText.text = Mathf.RoundToInt(currentGreen * 100f) + "%";

            yield return null;
        }
    }
}