using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class ChainGenerator : MonoBehaviour
{
    [Header("Shape")]
    public float totalLength = 0.6f;
    public float sag = 0.9f;

    [Header("Chain")]
    public GameObject linkPrefab;
    public int linkCount = 60;

    private List<GameObject> links = new List<GameObject>();

    void OnEnable() => UpdateChain();
    void OnValidate() => UpdateChain();

    void UpdateChain()
    {
        if (linkPrefab == null) return;

        // Ensure correct number of links
        while (links.Count < linkCount)
            links.Add(Instantiate(linkPrefab, transform));

        while (links.Count > linkCount)
        {
            DestroyImmediate(links[links.Count - 1]);
            links.RemoveAt(links.Count - 1);
        }

        float half = totalLength * 0.5f;

        // ---- SAMPLE CURVE ----
        int sampleCount = 300;
        List<Vector3> samples = new List<Vector3>();
        float totalArcLength = 0f;

        Vector3 prev = Vector3.zero;

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)(sampleCount - 1);

            float x = Mathf.Lerp(-half, half, t);
            float y = -4f * sag * t * (1 - t);

            Vector3 p = new Vector3(x, y, 0);
            samples.Add(p);

            if (i > 0)
                totalArcLength += Vector3.Distance(prev, p);

            prev = p;
        }

        float segmentLength = totalArcLength / (linkCount - 1);

        // ---- PLACE LINKS BY DISTANCE ----
        float accumulated = 0f;
        int sampleIndex = 0;

        for (int i = 0; i < linkCount; i++)
        {
            float targetDistance = i * segmentLength;

            float currentDistance = 0f;

            for (int j = 1; j < samples.Count; j++)
            {
                float d = Vector3.Distance(samples[j - 1], samples[j]);

                if (currentDistance + d >= targetDistance)
                {
                    float excess = targetDistance - currentDistance;
                    float ratio = excess / d;

                    Vector3 pos = Vector3.Lerp(samples[j - 1], samples[j], ratio);

                    links[i].transform.localPosition = pos;

                    // tangent
                    Vector3 tangent = (samples[j] - samples[j - 1]).normalized;
                    float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

                    Quaternion curveRot = Quaternion.Euler(0, 0, angle);

                    if (i % 2 == 0)
                        links[i].transform.localRotation = curveRot;
                    else
                        links[i].transform.localRotation = curveRot * Quaternion.Euler(90, 0, 0);

                    break;
                }

                currentDistance += d;
            }
        }
    }
}