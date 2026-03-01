using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackColor(0.2f, 0.8f, 1f)]
[TrackClipType(typeof(InteractableItemClip))]
[TrackBindingType(typeof(Renderer))]
public class InteractableItemTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<InteractableItemClipMixer>.Create(graph, inputCount);
    }
}