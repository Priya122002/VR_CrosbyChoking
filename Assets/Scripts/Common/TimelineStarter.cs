using UnityEngine;
using UnityEngine.Playables;

public class TimelineStarter : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public PlayableDirector normalTimeline;
    public PlayableDirector retryTimeline;

    private void Start()
    {
        if (RetryState.IsRetry)
        {
            Debug.Log("Retry detected → Playing Retry Timeline");

            RetryState.IsRetry = false; // Reset immediately
            PlayRetry();
        }
        else
        {
            Debug.Log("Normal start → Playing Normal Timeline");
            PlayNormal();
        }
    }

    public void PlayNormal()
    {
        if (retryTimeline != null)
            retryTimeline.Stop();

        normalTimeline.time = 0;
        normalTimeline.Play();
    }

    public void PlayRetry()
    {
        if (normalTimeline != null)
            normalTimeline.Stop();

        retryTimeline.time = 0;
        retryTimeline.Play();
    }
}