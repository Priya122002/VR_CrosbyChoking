using UnityEngine;
using System.Collections;

public class MaterialBlinkController : MonoBehaviour
{
    [Header("Blink Settings")]
    public Color blinkColor1 = Color.red;
    public Color blinkColor2 = Color.yellow;
    public float blinkSpeed = 0.5f;

    private Renderer objectRenderer;
    private Material runtimeMaterial;
    private Color defaultColor;

    private Coroutine blinkRoutine;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer found on this object.");
            return;
        }

        runtimeMaterial = objectRenderer.material;

        if (runtimeMaterial.HasProperty("_Color"))
        {
            defaultColor = runtimeMaterial.color;
        }
        else
        {
            Debug.LogWarning("Material does not have _Color property.");
        }
    }

    public void StartBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkMaterial());
    }

    public void ResetToDefault()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        if (runtimeMaterial.HasProperty("_Color"))
        {
            runtimeMaterial.color = defaultColor;
        }
    }

    private IEnumerator BlinkMaterial()
    {
        while (true)
        {
            runtimeMaterial.color = blinkColor1;
            yield return new WaitForSeconds(blinkSpeed);

            runtimeMaterial.color = blinkColor2;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}