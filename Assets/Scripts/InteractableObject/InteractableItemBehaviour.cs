using UnityEngine;
using UnityEngine.Playables;

public class InteractableItemBehaviour : PlayableBehaviour
{
    public Material highlightMaterial;

    private Renderer targetRenderer;

    private Material[] originalMaterials;
    private Material[] runtimeHighlightMaterials;

    private bool applied = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        targetRenderer = playerData as Renderer;

        if (targetRenderer == null || highlightMaterial == null)
            return;

        float weight = info.weight;

        if (weight > 0f)
        {
            if (!applied)
            {
                // Store original materials
                originalMaterials = targetRenderer.materials;

                // Create runtime material instances
                runtimeHighlightMaterials = new Material[originalMaterials.Length];

                for (int i = 0; i < runtimeHighlightMaterials.Length; i++)
                {
                    runtimeHighlightMaterials[i] = new Material(highlightMaterial);
                }

                targetRenderer.materials = runtimeHighlightMaterials;
                applied = true;
            }
        }
        else
        {
            RestoreOriginal();
        }
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // Restore ONLY if timeline actually stopped or exited clip
        if (info.effectivePlayState == PlayState.Playing)
            return;

        RestoreOriginal();
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        RestoreOriginal();
    }

    private void RestoreOriginal()
    {
        if (!applied || targetRenderer == null || originalMaterials == null)
            return;

        targetRenderer.materials = originalMaterials;

        if (runtimeHighlightMaterials != null)
        {
            for (int i = 0; i < runtimeHighlightMaterials.Length; i++)
            {
                if (Application.isPlaying)
                    Object.Destroy(runtimeHighlightMaterials[i]);
                else
                    Object.DestroyImmediate(runtimeHighlightMaterials[i]);
            }
        }

        applied = false;
    }
}