using UnityEngine;
using UnityEngine.UI;

public class BlinkUI : MonoBehaviour
{
    public Image targetImage;

    public Color colorA = Color.red;
    public Color colorB = Color.yellow;

    public float blinkSpeed = 2f; // higher = faster blink

    void Update()
    {
        if (targetImage == null) return;

        float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        targetImage.color = Color.Lerp(colorA, colorB, t);
    }
}