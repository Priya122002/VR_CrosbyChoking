using UnityEngine.Playables;

public class InteractableItemClipMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);

            if (weight <= 0f)
                continue;

            var inputPlayable = (ScriptPlayable<InteractableItemBehaviour>)playable.GetInput(i);
            var behaviour = inputPlayable.GetBehaviour();

            behaviour.ProcessFrame(inputPlayable, info, playerData);
        }
    }
}