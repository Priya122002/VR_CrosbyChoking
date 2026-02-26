using UnityEngine.Playables;
using UnityEngine;


public class SwitchPositionClip : PlayableAsset
{
    public Vector3 position;
    public Vector3 rotation; 
    public ExposedReference<Transform> parentTransform;


    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SwitchPositionBehaviour>.Create(graph);
        SwitchPositionBehaviour positionBehaviour = playable.GetBehaviour();

        positionBehaviour.position = position;
        positionBehaviour.rotation = Quaternion.Euler(rotation);
        positionBehaviour.parentTransform = parentTransform.Resolve(graph.GetResolver());
        
        return playable;
    }
}