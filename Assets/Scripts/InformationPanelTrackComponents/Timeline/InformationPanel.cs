using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Playables;

[TrackBindingType(typeof(GameObject))] // type of object the track can bind (since panel does not have its own class hence using as Gameobject)
[TrackClipType(typeof(InformationPanelClip))] // type of clip that can be added
public class InformationPanel : TrackAsset
{
    // this track is for the information panel, to change its basic color, and the text components
    //private Image ImageComp;
    //private Text Text;

    //private void Awake()
    //{
    //    ImageComp = 
    //}
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var binding = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        if (binding == null)
        {
            Debug.Log("Information panel track missing game object reference in timeline", go);
        }
        else
        {


            InformationPanelManager informationPanel;
            if (!binding.TryGetComponent(out informationPanel))
            {
                Debug.Log("[InformationPanel] found mismatch gameobject reference. Need InformationPanelManager");
            }
            else
            {
                // if (!binding.activeSelf) binding.SetActive(true);
                foreach (var c in GetClips())
                {
                    ((InformationPanelClip)(c.asset)).infoPanel = informationPanel;

                }
            }
        }

        return ScriptPlayable<InformationPanelBehaviour>.Create(graph, inputCount);
    }
}
