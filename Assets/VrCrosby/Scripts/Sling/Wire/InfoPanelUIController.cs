using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InfoPanelUIController : MonoBehaviour
{
    [Header("Panel")]
    public RectTransform panelRoot;

    [Header("Indicator Lines (Assign 4 Here)")]
    public List<Image> indicatorLines;

    [Header("Animation Settings")]
    public float targetScale = 0.0015f;
    public float popupDuration = 3f;
    public float lineFillDuration = 5f;

    Coroutine currentRoutine;

    void Awake()
    {
        panelRoot.localScale = Vector3.zero;

        foreach (var img in indicatorLines)
        {
            img.gameObject.SetActive(false);
            img.fillAmount = 0f;
        }

        gameObject.SetActive(false);
    }

    // 🔥 CALL THIS TO SHOW
    public void ShowPanel()
    {
        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(AnimateShow());
    }

    // 🔥 CALL THIS TO HIDE
    public void HidePanel()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(AnimateHide());
    }

    IEnumerator AnimateShow()
    {
        float t = 0f;

        // -------- FAST POPUP SCALE --------
        while (t < popupDuration)
        {
            float progress = t / popupDuration;

            // Smooth scale
            progress = Mathf.SmoothStep(0f, 1f, progress);

            float scaleValue = Mathf.Lerp(0f, targetScale, progress);
            panelRoot.localScale = Vector3.one * scaleValue;

            t += Time.deltaTime;
            yield return null;
        }

        panelRoot.localScale = Vector3.one * targetScale;

        // Enable indicator lines AFTER popup
        foreach (var img in indicatorLines)
        {
            img.gameObject.SetActive(true);
            img.fillAmount = 0f;
        }

        // -------- SLOW LINE FILL --------
        t = 0f;

        while (t < lineFillDuration)
        {
            float progress = t / lineFillDuration;

            // Smooth fill
            progress = Mathf.SmoothStep(0f, 1f, progress);

            foreach (var img in indicatorLines)
                img.fillAmount = progress;

            t += Time.deltaTime;
            yield return null;
        }

        foreach (var img in indicatorLines)
            img.fillAmount = 1f;
    }

    IEnumerator AnimateHide()
    {
        float t = 0f;

        // Reverse fill first
        while (t < lineFillDuration)
        {
            float progress = 1f - (t / lineFillDuration);
            progress = Mathf.SmoothStep(0f, 1f, progress);

            foreach (var img in indicatorLines)
                img.fillAmount = progress;

            t += Time.deltaTime;
            yield return null;
        }

        foreach (var img in indicatorLines)
        {
            img.fillAmount = 0f;
            img.gameObject.SetActive(false);
        }

        // -------- SCALE DOWN --------
        t = 0f;

        while (t < popupDuration)
        {
            float progress = 1f - (t / popupDuration);
            progress = Mathf.SmoothStep(0f, 1f, progress);

            float scaleValue = targetScale * progress;
            panelRoot.localScale = Vector3.one * scaleValue;

            t += Time.deltaTime;
            yield return null;
        }

        panelRoot.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}