using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SwitchPositionBehaviour : PlayableBehaviour
{
    // target position to jump/change the game object
    public Vector3 position;
    // target rotation of the game object
    public Quaternion rotation;
    // Game object will go under the following parent transform
    public Transform parentTransform = null;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        
        var clipTouchTime = playable.GetTime();

        if (info.effectivePlayState == PlayState.Playing && clipTouchTime <= 0.1f)
        {
            GameObject targetObject = playerData as GameObject;
            if (targetObject == null)
            {
                Debug.Log("SwitchPositionClip track missing game object reference in timeline");
                return;
            }
            targetObject.transform.SetParent(parentTransform);
            targetObject.transform.localPosition = position;
            targetObject.transform.rotation = rotation;
            targetObject.SetActive(true);

        }
    }
}