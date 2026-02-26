using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class InteractableItemClip : PlayableAsset
{
    public Material highlightMaterial;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<InteractableItemBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.highlightMaterial = highlightMaterial;

        return playable;
    }
}